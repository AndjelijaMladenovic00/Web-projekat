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


        [Route("UnosSobe/{hotelID}/{broj}/{br_kreveta}/{kategorija}")]
        [HttpPost]
        public async Task<ActionResult> UnesiSobu(int hotelID, int broj, int br_kreveta, int kategorija)
        {
            var Hotel=Context.Hoteli.Where(h => h.HotelID==hotelID).FirstOrDefault();
            if(Hotel==null) return BadRequest("Nepostojeci hotel!");

            var postojeca=Context.Sobe.Where(s => s.Hotel.HotelID == hotelID && s.BrojSobe == broj ).FirstOrDefault();
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

        [Route("PreuzimanjeSoba/{hotelID}")]
        [HttpGet]
        public async Task<ActionResult> PreuzmiSobe(int hotelID)
        {
            var hot=Context.Hoteli.Where(h=> h.HotelID==hotelID).FirstOrDefault();
            if(hot==null) return BadRequest("Nepostojeci hotel!");

            var sobe=await Context.Sobe.Where(s=>s.Hotel.HotelID==hotelID).Include(s=>s.Recepcioner).Include(s=>s.Gost).ToListAsync();

            if(sobe.Count==0) return BadRequest("Nema soba!");

            try
            {
                return Ok(
                    sobe.Select(s=>
                    new
                    {
                        SobaID=s.SobaID,
                        Broj=s.BrojSobe,
                        BrojKreveta=s.BrojKreveta,
                        Kategorija=s.Kategorija,
                        Izdata=s.Izdata,
                        Gost=s.Gost
                    }).ToList()
                );
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        [Route("PreuzimanjeSobaZaRecepcionera/{recepcionerID}")]
        [HttpGet]
        public async Task<ActionResult> PreuzmiSobeZaRecepcionera(int recepcionerID)
        {
            var sobe=await Context.Sobe.Where(s=>s.Recepcioner!=null && s.Recepcioner.RecepcionerID==recepcionerID).Include(s=>s.Gost).ToListAsync();

            try
            {
                return Ok(
                    sobe.Select(s=>
                    new
                    {
                        SobaID=s.SobaID,
                        Broj=s.BrojSobe,
                        BrojKreveta=s.BrojKreveta,
                        Kategorija=s.Kategorija,
                        Izdata=s.Izdata,
                        Gost=s.Gost
                    }).ToList()
                );
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        [Route("UklanjanjeSobe/{sobaID}")]
        [HttpDelete]
        public async Task<ActionResult> BrisanjeSobe(int sobaID)
        {

            var soba=Context.Sobe.Where(s => s.SobaID==sobaID).FirstOrDefault();
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

        [Route("PromenaBrojaKreveta/{sobaID}/{nbk}")]
        [HttpPut]
        public async Task<ActionResult> PromeniBrojKreveta(int sobaID, int nbk)
        {

            if(nbk<1||nbk>4) return BadRequest("Neodgovarajuci broj kreveta!");

            var soba=Context.Sobe.Where(s=>s.SobaID==sobaID).FirstOrDefault();
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

        [Route("PromenaKategorije/{sobaID}/{nk}")]
        [HttpPut]
        public async Task<ActionResult> PromeniKategoriju(int sobaID, int nk)
        {

            if(nk<1||nk>3) return BadRequest("Nepostojeca kategorija!");

            var soba=Context.Sobe.Where(s=>s.SobaID==sobaID).FirstOrDefault();
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