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
    public class HotelController:ControllerBase
    {
        public Context Context { get; set;}

        public HotelController(Context context)
        {
            Context=context;
        }

        [Route("UnosHotela/{ime}/{lokacija}/{spratovi}/{sobe}/{c1}/{c2}/{c3}")]
        [HttpPost]
        public async Task<ActionResult> UnesiHotel(string ime, string lokacija, int spratovi,int sobe, int c1, int c2, int c3)
        {
            if(ime=="") return BadRequest("Morate uneti ime hotela");
            if(ime.Length>70) return BadRequest("Predugacko ime hotela!");

            //ovo je radjeno kako dva hotela ne bi imala isto ime
            var istoIme=Context.Hoteli.Where(p => p.Naziv==ime).FirstOrDefault();
            if(istoIme!=null) return BadRequest("Naziv hotela mora biti jedinstven!");

            if(lokacija=="") return BadRequest("Morate uneti lokaciju hotela");
            if(lokacija.Length>200) return BadRequest("Predugacka lokacija!");

            if(spratovi<1 || spratovi>40) return BadRequest("Neodgovarajuci broj spratova!");

            if(sobe<1 || sobe>20) return BadRequest("Neodgovarajuci broj soba!");

            if(!((c1>c2)&&(c2>c3))) return BadRequest("Cene soba neodgovarajuce!");

            Hotel hotel=new Hotel();
            hotel.Naziv=ime;
            hotel.Lokacija=lokacija;
            hotel.BrojSpratova=spratovi;
            hotel.BrojSobaPoSpratu=sobe;
            hotel.Cena_I_kat=c1;
            hotel.Cena_II_kat=c2;
            hotel.Cena_III_kat=c3;

            try
            {
                Context.Hoteli.Add(hotel);
                await Context.SaveChangesAsync();
                return Ok("Hotel je dodat u bazu");

            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }

        }

       [Route("PreuzimanjeHotela")]
       [HttpGet]
       public async Task<ActionResult> PreuzmiHotele()
       {
           try
           {
               return Ok(await Context.Hoteli.Select(p => 
               new {ID=p.HotelID, Naziv=p.Naziv, Lokacija=p.Lokacija, 
               BrojSpratova=p.BrojSpratova, BrojSobaPoSpratu=p.BrojSobaPoSpratu,
               Cena_I_kat=p.Cena_I_kat,Cena_II_kat=p.Cena_II_kat,Cena_III_kat=p.Cena_III_kat, Recepcioneri=p.Recepcioneri, Sobe=p.Sobe}).ToListAsync());
           }
           catch(Exception e)
           {
               return BadRequest(e.Message);
           }
       }

       [Route("MenjanjeImenaHotela/{hotelID}/{novoIme}")]
       [HttpPut]
       public async Task<ActionResult> PromeniIme(int hotelID,string novoIme)
       {
           if(novoIme=="") return BadRequest("Morate uneti novo ime hotela");
           if(novoIme.Length>70) return BadRequest("Predugacko novo ime hotela");
           if(Context.Hoteli.Where(p => p.Naziv==novoIme).FirstOrDefault()!=null) return BadRequest("Hotel s ovim imenom vec postoji!");
           try
           {
               var hotel=Context.Hoteli.Where(p => p.HotelID==hotelID).FirstOrDefault();
               if(hotel!=null) hotel.Naziv=novoIme;
               else return BadRequest("Ne postoji trazeni hotel!");

               Context.Hoteli.Update(hotel);
               await Context.SaveChangesAsync();
               
               return Ok($"Uspesho promenjeno ime hotela-novo ime je {novoIme}");
           }
           catch(Exception e)
           {
               return BadRequest(e.Message);
           }
       }

       [Route("MenjanjeLokacije/{hotelID}/{novaLokacija}")]
       [HttpPut]
       public async Task<ActionResult> PromeniLokaciju(int hotelID,string novaLokacija)
       {
           if(novaLokacija=="") return BadRequest("Morate uneti novu lokaciju hotela");
           if(novaLokacija.Length>200) return BadRequest("Predugacka nova lokacija hotela");

           try
           {
               var hotel=Context.Hoteli.Where(p => p.HotelID==hotelID).FirstOrDefault();
               if(hotel!=null) hotel.Lokacija=novaLokacija;
               else return BadRequest("Ne postoji trazeni hotel!");

               Context.Hoteli.Update(hotel);
               await Context.SaveChangesAsync();
               
               return Ok($"Uspesho promenjena lokacija hotela-nova lokacija je {novaLokacija}");
           }
           catch(Exception e)
           {
               return BadRequest(e.Message);
           }
       }

       [Route("IzmenaBrojaSpratova/{hotelID}/{br}")]
       [HttpPut]
       public async Task<ActionResult> IzmeniBrojSpratova(int hotelID, int br)
       {
           if(br<0||br>40) return BadRequest("Neodgovarajuci broj spratova!");

           try
           {
               var hotel=Context.Hoteli.Where(p=>p.HotelID==hotelID).FirstOrDefault();
               if(hotel!=null) hotel.BrojSpratova=br;

               Context.Hoteli.Update(hotel);
               await Context.SaveChangesAsync();

               return Ok("Izmenjeni podaci o hotelu "+ hotel.Naziv);
           }
           catch(Exception e)
           {
               return BadRequest(e.Message);
           }
       }

       [Route("IzmenaBrojaSoba/{hotelID}/{br}")]
       [HttpPut]
       public async Task<ActionResult> IzmeniBrojSoba(int hotelID, int br)
       {
           if(br<0||br>20) return BadRequest("Neodgovarajuci broj soba po spratu!");

           try
           {
               var hotel=Context.Hoteli.Where(p=>p.HotelID==hotelID).FirstOrDefault();
               if(hotel!=null) hotel.BrojSobaPoSpratu=br;

               Context.Hoteli.Update(hotel);
               await Context.SaveChangesAsync();

               return Ok("Izmenjeni podaci o hotelu "+ hotel.Naziv);
           }
           catch(Exception e)
           {
               return BadRequest(e.Message);
           }
       }

       [Route("IzmenaCeneI/{hotelID}/{cI}")]
       [HttpPut]
       public async Task<ActionResult> IzmeniCenuI(int hotelID, int cI)
       {

           try
           {
               var hotel=Context.Hoteli.Where(p=>p.HotelID==hotelID).FirstOrDefault();
               if(hotel!=null) 
               {
                   if (cI<hotel.Cena_II_kat || cI<hotel.Cena_III_kat) return BadRequest("Neodgovarajuca cena sobe I kategorije");
                   hotel.Cena_I_kat=cI;
               }

               Context.Hoteli.Update(hotel);
               await Context.SaveChangesAsync();

               return Ok("Izmenjeni podaci o hotelu "+ hotel.Naziv);
           }
           catch(Exception e)
           {
               return BadRequest(e.Message);
           }
       }

       [Route("IzmenaCeneII/{hotelID}/{cII}")]
       [HttpPut]
       public async Task<ActionResult> IzmeniCenuII(int hotelID, int cII)
       {

           try
           {
               var hotel=Context.Hoteli.Where(p=>p.HotelID==hotelID).FirstOrDefault();
               if(hotel!=null) 
               {
                   if (cII<hotel.Cena_III_kat || cII>hotel.Cena_I_kat) return BadRequest("Neodgovarajuca cena sobe II kategorije");
                   hotel.Cena_II_kat=cII;
               }

               Context.Hoteli.Update(hotel);
               await Context.SaveChangesAsync();

               return Ok("Izmenjeni podaci o hotelu "+ hotel.Naziv);
           }
           catch(Exception e)
           {
               return BadRequest(e.Message);
           }
       }

       [Route("IzmenaCeneIII/{hotelID}/{cIII}")]
       [HttpPut]
       public async Task<ActionResult> IzmeniCenuIII(int hotelID, int cIII)
       {

           try
           {
               var hotel=Context.Hoteli.Where(p=>p.HotelID==hotelID).FirstOrDefault();
               if(hotel!=null) 
               {
                   if (cIII>hotel.Cena_II_kat || cIII>hotel.Cena_I_kat) return BadRequest("Neodgovarajuca cena sobe III kategorije");
                   hotel.Cena_III_kat=cIII;
               }

               Context.Hoteli.Update(hotel);
               await Context.SaveChangesAsync();

               return Ok("Izmenjeni podaci o hotelu "+ hotel.Naziv);
           }
           catch(Exception e)
           {
               return BadRequest(e.Message);
           }
       }

       //OVO PROVERI KAD BUDES NAPISALA SVE KONTROLERE

       [Route("DnevnaZaradaHotela/{hotelID}")]
       [HttpGet]
       public ActionResult DnevnaZaradaHotela(int hotelID)
       {
           try
           {
               var hotel=Context.Hoteli.Where(p => p.HotelID==hotelID).FirstOrDefault();
               
               if(hotel!=null)
               {
                   int I=0;
                   int II=0;
                   int III=0;
                
                   var iznajmljeneSobe=Context.Sobe.Where(s => s.Hotel==hotel && s.Gost!=null);
                
                   foreach(Soba s in iznajmljeneSobe)
                   {
                       
                           switch(s.Kategorija)
                           {
                                case 1: I++;
                                    break;
                                case 2: II++;
                                    break;
                                case 3: III++;
                                    break;
                           }  
                       
                   } 

                   int zarada=I*hotel.Cena_I_kat+II*hotel.Cena_II_kat+III*hotel.Cena_III_kat;

                   return Ok(zarada);  
               }
               else return BadRequest("Ne postoji trazeni hotel!");
           }
           catch(Exception e)
           {
               return BadRequest(e.Message);
           }
       }

    } 
}
