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

       [Route("BrisanjeHotela/{ime}")]
       [HttpDelete]
       public async Task<ActionResult> IzbrisiHotel(string ime)
       {
           if(ime.Length>70) return BadRequest("Ime predugacko da bi hotel postojao u bazi!");

           try
           {
               var hotel= Context.Hoteli.Where(p => p.Naziv==ime).FirstOrDefault();
               if(hotel!=null)
               {
                    Context.Hoteli.Remove(hotel);
                    await Context.SaveChangesAsync();
                    return Ok($"Hotel {ime} je obrisan");
               }
               else
               {
                   return Ok("Takav hotel nije ni postojao u bazi!");
               }

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
               Cena_I_kat=p.Cena_I_kat,Cena_II_kat=p.Cena_II_kat,Cena_III_kat=p.Cena_III_kat}).ToListAsync());
           }
           catch(Exception e)
           {
               return BadRequest(e.Message);
           }
       }

       [Route("MenjanjeImenaHotela/{staroIme}/{novoIme}")]
       [HttpPut]
       public async Task<ActionResult> PromeniIme(string staroIme,string novoIme)
       {
           if(novoIme=="") return BadRequest("Morate uneti novo ime hotela");
           if(novoIme.Length>70) return BadRequest("Predugacko novo ime hotela");
           if(Context.Hoteli.Where(p => p.Naziv==novoIme).FirstOrDefault()!=null) return BadRequest("Hotel s ovim imenom vec postoji!");
           try
           {
               var hotel=Context.Hoteli.Where(p => p.Naziv==staroIme).FirstOrDefault();
               if(hotel!=null) hotel.Naziv=novoIme;
               else return BadRequest("Ne postoji hotel " + staroIme);

               Context.Hoteli.Update(hotel);
               await Context.SaveChangesAsync();
               
               return Ok($"Uspesho promenjeno ime hotela {staroIme}-novo ime je {novoIme}");
           }
           catch(Exception e)
           {
               return BadRequest(e.Message);
           }
       }

       [Route("MenjanjeLokacije/{imeHotela}/{novaLokacija}")]
       [HttpPut]
       public async Task<ActionResult> PromeniLokaciju(string imeHotela,string novaLokacija)
       {
           if(novaLokacija=="") return BadRequest("Morate uneti novu lokaciju hotela");
           if(novaLokacija.Length>200) return BadRequest("Predugacka nova lokacija hotela");

           try
           {
               var hotel=Context.Hoteli.Where(p => p.Naziv==imeHotela).FirstOrDefault();
               if(hotel!=null) hotel.Lokacija=novaLokacija;
               else return BadRequest("Ne postoji hotel " + imeHotela);

               Context.Hoteli.Update(hotel);
               await Context.SaveChangesAsync();
               
               return Ok($"Uspesho promenjena lokacija hotela {hotel.Naziv}-nova lokacija je {novaLokacija}");
           }
           catch(Exception e)
           {
               return BadRequest(e.Message);
           }
       }

       [Route("IzmenaBrojaSpratova/{imeHotela}/{br}")]
       [HttpPut]
       public async Task<ActionResult> IzmeniBrojSpratova(string imeHotela, int br)
       {
           if(br<0||br>40) return BadRequest("Neodgovarajuci broj spratova!");

           try
           {
               var hotel=Context.Hoteli.Where(p=>p.Naziv==imeHotela).FirstOrDefault();
               if(hotel!=null) hotel.BrojSpratova=br;

               Context.Hoteli.Update(hotel);
               await Context.SaveChangesAsync();

               return Ok("Izmenjeni podaci o hotelu "+ imeHotela);
           }
           catch(Exception e)
           {
               return BadRequest(e.Message);
           }
       }

       [Route("IzmenaBrojaSoba/{imeHotela}/{br}")]
       [HttpPut]
       public async Task<ActionResult> IzmeniBrojSoba(string imeHotela, int br)
       {
           if(br<0||br>20) return BadRequest("Neodgovarajuci broj soba po spratu!");

           try
           {
               var hotel=Context.Hoteli.Where(p=>p.Naziv==imeHotela).FirstOrDefault();
               if(hotel!=null) hotel.BrojSobaPoSpratu=br;

               Context.Hoteli.Update(hotel);
               await Context.SaveChangesAsync();

               return Ok("Izmenjeni podaci o hotelu "+ imeHotela);
           }
           catch(Exception e)
           {
               return BadRequest(e.Message);
           }
       }

       [Route("IzmenaCeneI/{imeHotela}/{cI}")]
       [HttpPut]
       public async Task<ActionResult> IzmeniCenuI(string imeHotela, int cI)
       {

           try
           {
               var hotel=Context.Hoteli.Where(p=>p.Naziv==imeHotela).FirstOrDefault();
               if(hotel!=null) 
               {
                   if (cI<hotel.Cena_II_kat || cI<hotel.Cena_III_kat) return BadRequest("Neodgovarajuca cena sobe I kategorije");
                   hotel.Cena_I_kat=cI;
               }

               Context.Hoteli.Update(hotel);
               await Context.SaveChangesAsync();

               return Ok("Izmenjeni podaci o hotelu "+ imeHotela);
           }
           catch(Exception e)
           {
               return BadRequest(e.Message);
           }
       }

       [Route("IzmenaCeneII/{imeHotela}/{cII}")]
       [HttpPut]
       public async Task<ActionResult> IzmeniCenuII(string imeHotela, int cII)
       {

           try
           {
               var hotel=Context.Hoteli.Where(p=>p.Naziv==imeHotela).FirstOrDefault();
               if(hotel!=null) 
               {
                   if (cII<hotel.Cena_III_kat || cII>hotel.Cena_I_kat) return BadRequest("Neodgovarajuca cena sobe II kategorije");
                   hotel.Cena_II_kat=cII;
               }

               Context.Hoteli.Update(hotel);
               await Context.SaveChangesAsync();

               return Ok("Izmenjeni podaci o hotelu "+ imeHotela);
           }
           catch(Exception e)
           {
               return BadRequest(e.Message);
           }
       }

       [Route("IzmenaCeneIII/{imeHotela}/{cIII}")]
       [HttpPut]
       public async Task<ActionResult> IzmeniCenuIII(string imeHotela, int cIII)
       {

           try
           {
               var hotel=Context.Hoteli.Where(p=>p.Naziv==imeHotela).FirstOrDefault();
               if(hotel!=null) 
               {
                   if (cIII>hotel.Cena_II_kat || cIII>hotel.Cena_I_kat) return BadRequest("Neodgovarajuca cena sobe III kategorije");
                   hotel.Cena_III_kat=cIII;
               }

               Context.Hoteli.Update(hotel);
               await Context.SaveChangesAsync();

               return Ok("Izmenjeni podaci o hotelu "+ imeHotela);
           }
           catch(Exception e)
           {
               return BadRequest(e.Message);
           }
       }

       //OVO PROVERI KAD BUDES NAPISALA SVE KONTROLERE

       [Route("DnevnaZaradaHotela/{imeHotela}")]
       [HttpGet]
       public ActionResult DnevnaZaradaHotela(string imeHotela)
       {
           try
           {
               var hotel=Context.Hoteli.Where(p => p.Naziv==imeHotela).FirstOrDefault();
               
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

                   return Ok(I*hotel.Cena_I_kat+II*hotel.Cena_II_kat+III*hotel.Cena_III_kat);  
               }
               else return BadRequest("Ne postoji hotel "+imeHotela);
           }
           catch(Exception e)
           {
               return BadRequest(e.Message);
           }
       }

    } 
}