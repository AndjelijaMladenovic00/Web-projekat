import {Hotel} from "./Hotel.js";
import {Soba} from "./Soba.js";
import {Recepcioner} from "./Recepcioner.js";
import {Gost} from "./Gost.js";

let Hoteli=[];

function preuzmiHotele()
{
    fetch("https://localhost:5001/Hotel/PreuzimanjeHotela")
    .then(h=>{
        h.json().then(hoteli=>
            {
                hoteli.forEach(hot=> {

                    let hotel=new Hotel(hot.id, hot.naziv, hot.lokacija, hot.brojSpratova,
                        hot.brojSobaPoSpratu, hot.cena_I_kat, hot.cena_II_kat, hot.cena_III_kat);
                    
                    hot.sobe.forEach(s=>{
                        let soba=new Soba(s.sobaID, s.brojSobe, s.brojKreveta, s.kategorija, s.izdata);
                        hotel.Sobe.push(soba);
                    });

                    hot.recepcioneri.forEach(r=>{
                        let rec=new Recepcioner(r.recepcionerID, r.ime, r.prezime, r.iD_kartica);
                        hotel.Recepcioneri.push(rec);
                    });

                    Hoteli.push(hotel);
                });
                prikaziHotele();
                
            });
           
    });
}

function prikaziHotele(){
    Hoteli.forEach(h=>{
        h.prikaziHotel(document.body);
    });  
}

preuzmiHotele();


console.log(Hoteli);

