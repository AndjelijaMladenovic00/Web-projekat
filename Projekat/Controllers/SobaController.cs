using Microsoft.AspNetCore.Mvc;
using Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Projekat.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SobaController:ControllerBase
    {
        public Context Context { get; set; }

        public SobaController(Context context)
        {
            Context = context;
        }


        [Route("UnosSobe/{hotel}/{broj}/{br_kreveta}/{kategorija}")]
        [HttpPost]
        public async Task<ActionResult> UnesiSobu(string hotel, int broj, int br_kreveta, int kategorija)
        {
            var Hotel=Context.Hoteli.Where(h => h.Naziv==hotel).FirstOrDefault();
            if(Hotel==null) return BadRequest("Nepostojeci hotel!");

            var postojeca=Context.Sobe.Where(s => s.Hotel.Naziv == hotel && s.BrojSobe == broj ).FirstOrDefault();
            if(postojeca!=null) return BadRequest("U ovom hotelu vec postoji soba s brojem "+broj+".");

            if(broj<0 || Hotel.BrojSpratova*Hotel.BrojSobaPoSpratu<broj) return BadRequest("Broj sobe neodgovarajuci!");
            
            if(br_kreveta<1||br_kreveta>4) return BadRequest("Neodgovarajuci broj kreveta!");

            if(kategorija<1||kategorija>3) return BadRequest("Neodgovarajuca kategorija!");

            Soba soba=new Soba();
            soba.BrojSobe=broj;
            soba.BrojKreveta=br_kreveta;
            soba.Kategorija=kategorija;
            soba.Hotel=Hotel;
            soba.Izdata=false;

            try
            {
                Context.Add(soba);
                await Context.SaveChangesAsync();
                return Ok("Soba je dodata.");

            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("PreuzimanjeSoba/{hotel}")]
        [HttpGet]
        public async Task<ActionResult> PreuzmiSobe(string hotel)
        {
            var hot=Context.Hoteli.Where(h=> h.Naziv==hotel).FirstOrDefault();
            if(hot==null) return BadRequest("Nepostojeci hotel!");

            var sobe=await Context.Sobe.Where(s=>s.Hotel.Naziv==hotel).Include(s=>s.Recepcioner).ToListAsync();

            if(sobe.Count==0) return BadRequest("Nema soba!");

            try
            {
                return Ok(sobe);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        [Route("UklanjanjeSobe/{hotel}/{broj}")]
        [HttpDelete]
        public async Task<ActionResult> BrisanjeSobe(string hotel, int broj)
        {
            if(hotel.Length>70 || broj<0) return BadRequest("Neodgovarajuci podaci o sobi!") ;

            var soba=Context.Sobe.Where(s => s.Hotel.Naziv==hotel && s.BrojSobe==broj).FirstOrDefault();
            if(soba==null) return Ok("Soba nije ni postojala!");

            else try
            {
                Context.Remove(soba);
                await Context.SaveChangesAsync();
                return Ok("Soba obrisana!");
            }   
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }       
        }

        [Route("PromenaBrojaKreveta/{hotel}/{brSobe}/{nbk}")]
        [HttpPut]
        public async Task<ActionResult> PromeniBrojKreveta(string hotel, int brSobe, int nbk)
        {
            if(hotel.Length==0 || hotel.Length>70) return BadRequest("Neodgovarajuce ime hotela!");

            if(nbk<1||nbk>4) return BadRequest("Neodgovarajuci broj kreveta!");

            var Hotel=Context.Hoteli.Where(h => h.Naziv==hotel).Include(h => h.Sobe).FirstOrDefault();
            if(Hotel==null) return BadRequest("Nepostojeci hotel!");

            Soba soba=null;

            foreach(Soba s in Hotel.Sobe)
            {
                if(s.BrojSobe==brSobe)
                {
                    soba=s;
                    break;
                }
            }
            if(soba==null) return BadRequest("Nepostojeca soba!");

            soba.BrojKreveta=nbk;

            try
            {
                Context.Update(soba);
                await Context.SaveChangesAsync();
                return Ok("Update-ovana soba!");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("PromenaKategorije/{hotel}/{brSobe}/{nk}")]
        [HttpPut]
        public async Task<ActionResult> PromeniKategoriju(string hotel, int brSobe, int nk)
        {
            if(hotel.Length==0 || hotel.Length>70) return BadRequest("Neodgovarajuce ime hotela!");

            if(nk<1||nk>3) return BadRequest("Nepostojeca kategorija!");

            var Hotel=Context.Hoteli.Where(h => h.Naziv==hotel).Include(h => h.Sobe).FirstOrDefault();
            if(Hotel==null) return BadRequest("Nepostojeci hotel!");

            Soba soba=null;

            foreach(Soba s in Hotel.Sobe)//ovo ujedno proverava i da li je ok broj sobe
            {
                if(s.BrojSobe==brSobe)
                {
                    soba=s;
                    break;
                }
            }
            if(soba==null) return BadRequest("Nepostojeca soba!");

            soba.Kategorija=nk;

            try
            {
                Context.Update(soba);
                await Context.SaveChangesAsync();
                return Ok("Update-ovana soba!");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}