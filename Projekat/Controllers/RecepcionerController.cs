using Microsoft.AspNetCore.Mvc;
using Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

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

        [Route("UnosRecepcionera/{ime}/{prezime}/{id}/{hotelID}")]
        [HttpPost]
        public async Task<ActionResult> DodajRecepcionera(string ime, string prezime, string id, int hotelID)
        {
            if(ime.Length==0 || ime.Length>50) return BadRequest("Neodgovarajuce ime!");

            if(prezime.Length==0 || prezime.Length>50) return BadRequest("Neodgovarajuce prezime!");

            if(id.Length!=5) return BadRequest("Neodgovarajuci ID!");
            foreach(char c in id.ToCharArray())
            {
                if(c<'0'||c>'9') return BadRequest("Broj ID kartice treba da se sastoji samo iz cifara!");
            }

            var Hotel=Context.Hoteli.Where(h => h.HotelID==hotelID).Include(h => h.Recepcioneri).FirstOrDefault();
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
                return Ok(rec);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        [Route("UklanjanjeRecepcionera/{id}")]
        [HttpDelete]
        public async Task<ActionResult>UkloniRecepcionera(int id)
        {
            Recepcioner rec=Context.Recepcioneri.Where(r=>r.RecepcionerID==id).FirstOrDefault();

            if(rec==null) return Ok("Recepcioner nije ni bio u bazi!");

            Hotel h=Context.Hoteli.Where(h=>h.HotelID==rec.Hotel.HotelID).FirstOrDefault();

            if(rec.IzdateSobe.Count!=0)
            {
                Recepcioner zamena=null;
                foreach(Recepcioner r in h.Recepcioneri)
                {
                    if(r.RecepcionerID!=id) zamena=r;
                    if(zamena!=null) break;
                }

                foreach(Soba s in rec.IzdateSobe)
                {
                    s.Recepcioner=zamena;
                }
            }

            
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

        [Route("PreuzimanjeRecepcionera/{hotelID}")]
        [HttpGet]
        public async Task<ActionResult> PreuzmiRecepcionere(int hotelID)
        {
            var hot=Context.Hoteli.Where(h=>h.HotelID==hotelID).FirstOrDefault();

            if(hot==null) return BadRequest("Nepostojeci hotel!");

            var recepcioneri= await Context.Recepcioneri.Where(p=> p.Hotel.HotelID==hotelID).Include(r=> r.IzdateSobe).ToListAsync();

            try
            {
                return Ok(recepcioneri);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("IzdavanjeSobe/{hotelID}/{id}/{brs}/{blk}")]
        [HttpPut]
        public async Task<ActionResult> IzdajSobu(int hotelID, string id, int brs, string blk)
        {

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

            var Hotel=Context.Hoteli.Where(h => h.HotelID==hotelID).Include(h => h.Sobe).Include(h => h.Recepcioneri).Include(h => h.Gosti).FirstOrDefault();
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
            if(soba.Izdata==true) return BadRequest("Soba je vec izdata!");

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

        [Route("IzmenaBrojaIDKartice/{recID}/{noviBroj}")]
        [HttpPut]
        public async Task<ActionResult> IzmeniBrojIDKartice(int recID, string noviBroj)
        {
            if(noviBroj.Length!=5) return BadRequest("Nevalidni brojevi ID kartica!");
            
            foreach(char c in noviBroj.ToCharArray())
            {
                if(c>'9'||c<'0') return BadRequest("Broj ID karte moze da sadrzi samo cifre!");
            }

            var recepcioner=Context.Recepcioneri.Where(p => p.RecepcionerID==recID).FirstOrDefault();
            if(recepcioner==null) return BadRequest("Ne postoji trazeni recepcioner");
            recepcioner.ID_kartica=noviBroj;

            try
            {
                Context.Recepcioneri.Update(recepcioner);
                await Context.SaveChangesAsync();

                return Ok(recepcioner);

            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("IzmenaImenaRecepcionera/{id}/{novoIme}")]
        [HttpPut]
        public async Task<ActionResult> IzmeniImeRecepcionera(int id, string novoIme)
        {

            if(novoIme.Length>50) return BadRequest("Ime predugacko!");

            var rec=Context.Recepcioneri.Where(g => g.RecepcionerID==id).FirstOrDefault();
            if(rec==null) return BadRequest("Ne postoji trazeni recepcioner!");

            rec.Ime=novoIme;

            try
            {
                Context.Update(rec);
                await Context.SaveChangesAsync();

                return Ok(rec);

            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("IzmenaPrezimenaRecepcionera/{id}/{novoPrezime}")]
        [HttpPut]
        public async Task<ActionResult> IzmeniPrezimeRecepcionera(int id, string novoPrezime)
        {

            if(novoPrezime.Length>50) return BadRequest("Ime predugacko!");

            var rec=Context.Recepcioneri.Where(g => g.RecepcionerID==id).FirstOrDefault();
            if(rec==null) return BadRequest("Ne postoji trazeni recepcioner!");

            rec.Prezime=novoPrezime;

            try
            {
                Context.Update(rec);
                await Context.SaveChangesAsync();

                return Ok(rec);

            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }

        }
            
        [Route("IzmenaPodataka/{recID}/{noviBroj}/{novoIme}/{novoPrezime}")]
        [HttpPut]
        public async Task<ActionResult> IzmeniPodatkeRecepcioneru(int recID, string noviBroj, string novoIme, string novoPrezime)
        {
            if(noviBroj.Length!=5) return BadRequest("Nevalidni brojevi ID kartica!");
            
            foreach(char c in noviBroj.ToCharArray())
            {
                if(c>'9'||c<'0') return BadRequest("Broj ID karte moze da sadrzi samo cifre!");
            }

            if(novoIme.Length==0||novoIme.Length>50) return BadRequest("Neodgovarajuce ime!");
            if(novoPrezime.Length==0||novoPrezime.Length>50) return BadRequest("Neodgovarajuce prezime!");

            var recepcioner=Context.Recepcioneri.Where(p => p.RecepcionerID==recID).FirstOrDefault();
            if(recepcioner==null) return BadRequest("Ne postoji trazeni recepcioner");

            recepcioner.ID_kartica=noviBroj;
            recepcioner.Ime=novoIme;
            recepcioner.Prezime=novoPrezime;

            try
            {
                Context.Recepcioneri.Update(recepcioner);
                await Context.SaveChangesAsync();

                return Ok(recepcioner);

            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
    }
}