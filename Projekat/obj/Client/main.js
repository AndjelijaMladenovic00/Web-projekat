import {Hotel} from "./Hotel.js";
import {Soba} from "./Soba.js";
import {Recepcioner} from "./Recepcioner.js";
import {Gost} from "./Gost.js";

let Hoteli=[];

fetch("https://localhost:5001/Hotel/PreuzimanjeHotela")
.then(h=>{
    h.json().then(hoteli=>{
        hoteli.forEach(hotel => {
                let h=new Hotel(hotel.id,hotel.naziv, hotel.lokacija, hotel.brojSpratova, hotel.brojSobaPoSpratu, 
                    hotel.cena_I_kat, hotel.cena_II_kat, hotel.cena_III_kat);
                
                Hoteli.push(h);
                
            })
    })
})

console.log(Hoteli);

