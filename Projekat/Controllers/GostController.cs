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

        [Route("DodavanjeGosta/{ime}/{prezime}/{blk}/{hotel}")]
        [HttpPost]  
        public async Task<ActionResult> DodajGosta(string ime, string prezime, string blk, string hotel)
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

            if(hotel.Length>70) return BadRequest("Neodgovarajuce ime hotela!");
            var Hotel=Context.Hoteli.Where(h => h.Naziv==hotel).FirstOrDefault();
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

        [Route("IzmenaBrojaLK/{stariBroj}/{noviBroj}")]
        [HttpPut]
        public async Task<ActionResult> IzmeniBrojLK(string stariBroj, string noviBroj)
        {
            if(stariBroj.Length!=9 ||noviBroj.Length!=9) return BadRequest("Nevalidni brojevi lk!");

            foreach(char c in stariBroj.ToCharArray())
            {
                if(c>'9'||c<'0') return BadRequest("Broj licne karte moze da sadrzi samo cifre!");
            }

            
            foreach(char c in noviBroj.ToCharArray())
            {
                if(c>'9'||c<'0') return BadRequest("Broj licne karte moze da sadrzi samo cifre!");
            }

            var gost=Context.Gosti.Where(p => p.BrojLicneKarte==stariBroj).FirstOrDefault();
            if(gost==null) return BadRequest("Ne postoji gost sa brojem lk "+stariBroj);
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

        [Route("IzmenaImenaGosta/{blk}/{novoIme}")]
        [HttpPut]
        public async Task<ActionResult> IzmeniImeGosta(string blk, string novoIme)
        {
            foreach(char c in blk.ToCharArray())
            {
                if(c>'9'||c<'0') return BadRequest("Broj licne karte moze da sadrzi samo cifre!");
            }

            if(novoIme.Length>50) return BadRequest("Ime predugacko!");

            var gost=Context.Gosti.Where(g => g.BrojLicneKarte==blk).FirstOrDefault();
            if(gost==null) return BadRequest("Ne postoji gost sa brojem licne karte "+blk);

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

        [Route("IzmenaPrezimenaGosta/{blk}/{novoPrezime}")]
        [HttpPut]
        public async Task<ActionResult> IzmeniPrezimeGosta(string blk, string novoPrezime)
        {
            foreach(char c in blk.ToCharArray())
            {
                if(c>'9'||c<'0') return BadRequest("Broj licne karte moze da sadrzi samo cifre!");
            }

            if(novoPrezime.Length>50) return BadRequest("Ime predugacko!");

            var gost=Context.Gosti.Where(g => g.BrojLicneKarte==blk).FirstOrDefault();
            if(gost==null) return BadRequest("Ne postoji gost sa brojem licne karte "+blk);

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

        [Route("BrisanjeGosta/{blk}")]
        [HttpDelete]
        public async Task<ActionResult> ObrisiGosta(string blk)
        {
            if(blk.Length!=9) return BadRequest("Neodgovarajuca duzina broja licne karte!");

            foreach(char c in blk.ToCharArray())
            {
                if(c<'0'|| c>'9') return BadRequest("Broj licne karte se sastoji samo od cifara!");
            }

            var gost=Context.Gosti.Where(g => g.BrojLicneKarte==blk).FirstOrDefault();
            if(gost==null) return Ok("Gost ne postoji!");

            try
            {
                Context.Gosti.Remove(gost);
                await Context.SaveChangesAsync();
                return Ok("Gost obrisan!");
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
            return Ok(gost);
        }

    }

}