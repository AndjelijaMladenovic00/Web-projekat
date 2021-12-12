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

    public class RecepcionerController:ControllerBase
    {
        public Context Context { get; set; }

        public RecepcionerController(Context context)
        {
            Context = context;
        }

        [Route("UnosRecepcionera/{ime}/{prezime}/{id}/{hotel}")]
        [HttpPost]
        public async Task<ActionResult> DodajRecepcionera(string ime, string prezime, string id, string hotel)
        {
            if(ime.Length==0 || ime.Length>50) return BadRequest("Neodgovarajuce ime!");

            if(prezime.Length==0 || prezime.Length>50) return BadRequest("Neodgovarajuce prezime!");
            
            if(hotel.Length==0 || hotel.Length>70) return BadRequest("Neodgovarajuce ime hotela!");

            if(id.Length!=5) return BadRequest("Neodgovarajuci ID!");
            foreach(char c in id.ToCharArray())
            {
                if(c<'0'||c>'9') return BadRequest("Broj ID kartice treba da se sastoji samo iz cifara!");
            }

            var Hotel=Context.Hoteli.Where(h => h.Naziv==hotel).Include(h => h.Recepcioneri).FirstOrDefault();
            if(Hotel==null) return BadRequest("Nepostojeci hotel!");

            foreach(Recepcioner r in Hotel.Recepcioneri) //1 recepcioner po hotelu sa datim id-em
            {
                if(r.ID_kartica==id) return BadRequest("Ne mogu dva recepcionera imati isti broj ID kartice!");
            }

            Recepcioner rec=new Recepcioner();
            rec.Ime=ime;
            rec.Prezime=prezime;
            rec.ID_kartica=id;
            rec.Hotel=Hotel;

            try
            {
                Context.Add(rec);
                await Context.SaveChangesAsync();
                return Ok("Dodat recepcioner!");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        [Route("Uklanjanje recepcionera/{hotel}/{id}")]
        [HttpDelete]
        public async Task<ActionResult>UkloniRecepcionera(string hotel, string id)
        {
            if(hotel.Length==0||hotel.Length>70) return BadRequest("Neodgovarajuce ime hotela!");

            if(id.Length!=5) return BadRequest("Neodgovarajuci broj ID kartice!");
            foreach(char c in id.ToCharArray())
            {
                if(c<'0'||c>'9') return BadRequest("ID treba da se sastoji samo iz brojeva!");
            }

            var Hotel=Context.Hoteli.Where(h => h.Naziv==hotel).Include(h => h.Recepcioneri).FirstOrDefault();
            if(Hotel==null) return BadRequest("Nepostojeci hotel!");

            Recepcioner rec=null;

            foreach(Recepcioner r in Hotel.Recepcioneri)
            {
                if(r.ID_kartica==id)
                {
                    rec=r;
                    break;
                }
                
            }
            if(rec==null) return Ok("Recepcioner nije ni bio u bazi!");

            try
            {
                Context.Remove(rec);
                await Context.SaveChangesAsync();
                return Ok("Recepcioner uklonjen!");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("PreuzimanjeRecepcionera/{hotel}")]
        [HttpGet]
        public async Task<ActionResult> PreuzmiRecepcionere()
        {
            try
            {
                return Ok(await Context.Recepcioneri.Select(p=>
                new {RecepcionerID=p.RecepcionerID, Ime=p.Ime,Prezime=p.Prezime,
                Id_kartica=p.ID_kartica, IzdateSobe=p.IzdateSobe }).ToListAsync());
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("IzdavanjeSobe/{hotel}/{id}/{brs}")]
        [HttpPut]
        public async Task<ActionResult> IzdajSobu(string hotel, string id, int brs, string blk)
        {
            if(hotel.Length==0||hotel.Length>70) return BadRequest("Neodgovarajuce ime hotela!");

            if(id.Length!=5) return BadRequest("Neodgovarajuci broj ID kartice!");
            foreach(char c in id.ToCharArray())
            {
                if(c<'0'||c>'9') return BadRequest("ID treba da se sastoji samo iz brojeva!");
            }

            if(blk.Length!=9) return BadRequest("Neodgovarajuci broj licne karte!");

            foreach(char c in blk.ToCharArray())
            {
                if(c<'0'||c>'9') return BadRequest("Broj licne karte treba da se sastoji samo iz brojeva!");
            }

            var Hotel=Context.Hoteli.Where(h => h.Naziv==hotel).Include(h => h.Sobe).Include(h => h.Recepcioneri).Include(h => h.Gosti).FirstOrDefault();
            if(Hotel==null)return BadRequest("Nepostojeci hotel");

            Recepcioner rec=null;

            foreach(Recepcioner r in Hotel.Recepcioneri)
            {
                if(r.ID_kartica==id)
                {
                    rec=r;
                    break;
                }
            }
            if(rec==null) return BadRequest("Nepostojeci recepcioner!");

            Soba soba=null;
            foreach(Soba s in Hotel.Sobe)
            {
                if(s.BrojSobe==brs)
                {
                    soba=s;
                    break;
                }
            }
            if(soba==null) return BadRequest("Nepostojeca soba!");

            Gost gost=null;
            foreach(Gost g in Hotel.Gosti)
            {
                if(g.BrojLicneKarte==blk)
                {
                    gost=g;
                    break;
                }
            }
            if(gost==null) return BadRequest("Nepostojeci gost!");

            soba.Izdata=true;
            soba.Recepcioner=rec;
            soba.Gost=gost;

            try
            {
                Context.Update(soba);
                await Context.SaveChangesAsync();
                return Ok("Soba je izdata!");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
            
        }
        
    }
}