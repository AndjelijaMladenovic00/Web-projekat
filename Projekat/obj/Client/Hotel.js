import { Recepcioner } from "./Recepcioner.js";
import {Soba} from "./Soba.js"
import { Gost } from "./Gost.js";

export class Hotel{

    constructor(id, naziv, lokacija, brSpratova, brSobaPoSpratu, cenaI, cenaII, cenaIII){
        this.id=id;
        this.naziv=naziv;
        this.lokacija=lokacija;
        this.brSpratova=brSpratova;
        this.brSobaPoSpratu=brSobaPoSpratu;
        this.cenaI=cenaI;
        this.cenaII=cenaII;
        this.cenaIII=cenaIII;
        this.Sobe=[];
        this.Recepcioneri=[];
        this.kontejner=null;

        fetch("https://localhost:5001/Soba/PreuzimanjeSoba/"+this.naziv)
        .then(s=>{
            s.json().then(sobe=>{
                sobe.forEach(s=>{
                    let soba=new Soba(s.sobaID, s.brojSobe, s.brojKreveta, s.kategorija, s.izdata);
                    if(soba.izdata===true){
                        fetch("https://localhost:5001/Gost/PreuzmiGosta/"+this.naziv+"/"+soba.broj)
                        .then(g=>{
                            g.json().then(gs=>{
                                let gost=new Gost(gs.gostID, gs.ime, gs.prezime, gs.brojLicneKarte);
                                soba.gost=gost;
                            })
                        })
                    }

                    this.Sobe.push(soba);
                })
            })
        })

        fetch("https://localhost:5001/Recepcioner/PreuzimanjeRecepcionera/"+this.naziv)
        .then(r=>{
            r.json().then(recepcioneri=>{
                recepcioneri.forEach(recep => {
                    let rec=new Recepcioner(recep.recepcionerID, recep.ime, recep.prezime, recep.iD_kartica);
                    recep.izdateSobe.forEach(is=>{
                        id=is.sobaID;
                        this.Sobe.forEach(s=>{
                            if(s.id==id){
                                console.log("postoji");
                                rec.Sobe.push(s);
                            }
                        })
                    })
                    this.Recepcioneri.push(rec);
                    
                })
                this.prikaziHotel(document.body);
             })
        })

        

    }

    prikaziHotel(host){

        this.kontejner=document.createElement("div");
        this.kontejner.className="kontejner";
        host.appendChild(this.kontejner);

        let meniKontejner=document.createElement("div");
        meniKontejner.className="meniKontejner";
        this.kontejner.appendChild(meniKontejner);
        this.prikaziMeni(meniKontejner);

        console.log(this.kontejner);


    }

    prikaziMeni(host){

        let labela=document.createElement("label");
        labela.innerHTML="Recepcioner:";
        host.appendChild(labela);

        let se=document.createElement("select");
        host.appendChild(se);

        let op;
        this.Recepcioneri.forEach(rec=>{
            op=document.createElement("option");
            op.innerHTML=rec.ime+" "+rec.prezime;
            op.value=rec.id;
            se.appendChild(op);
        })

    }
}