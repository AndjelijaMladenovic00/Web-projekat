using Microsoft.AspNetCore.Mvc;
using Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace Projekat.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GostController:ControllerBase
    {
        public Context Context { get; set; }

        public GostController(Context context)
        {
            Context=context;
        }

        [Route("DodavanjeGosta/{ime}/{prezime}/{blk}/{hotelID}")]
        [HttpPost]  
        public async Task<ActionResult> DodajGosta(string ime, string prezime, string blk, int hotelID)
        {
            if(ime=="") return BadRequest("Morate uneti ime gosta!");
            if(ime.Length>50) return BadRequest("Predugacko ime");

            if(prezime=="") return BadRequest("Morate uneti prezime gosta!");
            if(prezime.Length>50) return BadRequest("Predugacko prezime");

            if(blk.Length!=9) return BadRequest("Neodgovarajuca duzina broja licne karte");
            foreach(char c in blk.ToCharArray())
            {
                    if(c>'9'||c<'0') return BadRequest("Broj licne karte se sastoji samo od cifara!");
            }

            var Hotel=Context.Hoteli.Where(h => h.HotelID==hotelID).FirstOrDefault();
            if(Hotel==null) return BadRequest("Ne postoji navedeni hotel!");

            //validacija da ne postoji osoba sa tim brojem licne karte vec u bazi
            var IstaLK= Context.Gosti.Where(p => p.BrojLicneKarte==blk).FirstOrDefault();
            if(IstaLK!=null) return BadRequest("Dva gosta ne mogu imati isti broj licne karte!");

            Gost gost=new Gost();
            gost.Ime=ime;
            gost.Prezime=prezime;
            gost.BrojLicneKarte=blk;
            gost.Hotel=(Hotel)Hotel;

            try
            {
                Context.Gosti.Add(gost);
                await Context.SaveChangesAsync();
                return Ok($"Dodat gost {ime} {prezime}");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("IzmenaBrojaLK/{gostID}/{noviBroj}")]
        [HttpPut]
        public async Task<ActionResult> IzmeniBrojLK(int gostID, string noviBroj)
        {
            if(noviBroj.Length!=9) return BadRequest("Nevalidni brojevi lk!");

            foreach(char c in noviBroj.ToCharArray())
            {
                if(c>'9'||c<'0') return BadRequest("Broj licne karte moze da sadrzi samo cifre!");
            }

            var gost=Context.Gosti.Where(p => p.GostID==gostID).FirstOrDefault();
            if(gost==null) return BadRequest("Ne postoji trazeni gost!");

            var IstaLK= Context.Gosti.Where(p => p.BrojLicneKarte==noviBroj).FirstOrDefault();
            if(IstaLK!=null) return BadRequest("Dva gosta ne mogu imati isti broj licne karte!");

            gost.BrojLicneKarte=noviBroj;

            try
            {
                Context.Gosti.Update(gost);
                await Context.SaveChangesAsync();

                return Ok("Izmenjeni podaci o gostu!");

            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("IzmenaImenaGosta/{gostID}/{novoIme}")]
        [HttpPut]
        public async Task<ActionResult> IzmeniImeGosta(int gostID, string novoIme)
        {
            if(novoIme.Length>50) return BadRequest("Ime predugacko!");

            var gost=Context.Gosti.Where(g => g.GostID==gostID).FirstOrDefault();
            if(gost==null) return BadRequest("Ne postoji trazeni gost!");

            gost.Ime=novoIme;

            try
            {
                Context.Update(gost);
                await Context.SaveChangesAsync();

                return Ok("Izmenjeni podaci o gostu!");

            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("IzmenaPrezimenaGosta/{gostID}/{novoPrezime}")]
        [HttpPut]
        public async Task<ActionResult> IzmeniPrezimeGosta(int gostID, string novoPrezime)
        {
            if(novoPrezime.Length>50) return BadRequest("Prezime predugacko!");

            var gost=Context.Gosti.Where(g => g.GostID==gostID).FirstOrDefault();
            if(gost==null) return BadRequest("Ne postoji trazeni gost!");

            gost.Prezime=novoPrezime;

            try
            {
                Context.Update(gost);
                await Context.SaveChangesAsync();

                return Ok("Izmenjeni podaci o gostu!");

            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        [Route("BrisanjeGosta/{gostID}")]
        [HttpDelete]
        public async Task<ActionResult> ObrisiGosta(int gostID)
        {
            var gost=Context.Gosti.Where(g => g.GostID==gostID).FirstOrDefault();
            if(gost==null) return Ok("Gost ne postoji!");

            if(gost.Soba!=null && gost.Soba.Count!=0)
            {
                foreach(Soba s in gost.Soba)
                {
                    s.Gost=null;
                    s.Izdata=false;
                }
            }

            try
            {
                Context.Gosti.Remove(gost);
                await Context.SaveChangesAsync();
                return Ok(gost);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("NadjiGosta/{blk}")]
        [HttpGet]
        public ActionResult NadjiGosta(string blk)
        {
            if(blk.Length!=9) return BadRequest("Neodgovarajuca duzina broja licne karte!");

            foreach(char c in blk.ToCharArray())
            {
                if(c<'0'|| c>'9') return BadRequest("Broj licne karte se sastoji samo od cifara!");
            }

            var gost=Context.Gosti.Where(g => g.BrojLicneKarte==blk).Include(g => g.Hotel).Include(g => g.Soba).FirstOrDefault();
            
            if(gost==null) return BadRequest("Gost nepostojeci!");
            return Ok(gost);
        }

        [Route("PreuzmiGosta/{hotel}/{brSobe}")]
        [HttpGet]
        public async Task<ActionResult> PreuzimanjeGosta(string hotel, int brSobe)
        {
            var hot=Context.Hoteli.Where(h=>h.Naziv==hotel).FirstOrDefault();
            if(hot==null) return BadRequest("Nepostojeci hotel!");

            var gosti=await Context.Gosti.Where(g=> g.Hotel.Naziv==hotel).Include(g=> g.Soba).ToListAsync();
            if(gosti==null || gosti.Count==0) return BadRequest("Ovakvi podaci ne postoje!");

            Gost gost=null;

            foreach(Gost g in gosti)
            {
                if(g.Soba.Count==0) continue;
                foreach(Soba s in g.Soba)
                {
                    if(s.BrojSobe==brSobe)
                    {
                        gost=g;
                        break;
                    }
                }
                if(g!=null) break;
            }
            return Ok(gost);
        }

    }

}