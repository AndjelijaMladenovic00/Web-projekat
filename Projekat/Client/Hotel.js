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
                            });
                        });
                    }

                    this.Sobe.push(soba);
                });

                fetch("https://localhost:5001/Recepcioner/PreuzimanjeRecepcionera/"+this.naziv)
                .then(r=>{
                    r.json().then(recepcioneri=>{
                        recepcioneri.forEach(recep => {
                            let rec=new Recepcioner(recep.recepcionerID, recep.ime, recep.prezime, recep.iD_kartica);
                            recep.izdateSobe.forEach(is=>{
                                id=is.sobaID;
                                this.Sobe.forEach(s=>{
                                    if(s.id===id){
                                        
                                        rec.Sobe.push(s);
                                    }
                                });
                            });
                            this.Recepcioneri.push(rec); 
                        });

                        this.prikaziHotel(document.body);
                        
                     });
                     
                     
                });

            });

        });

        
    }

    prikaziHotel(host){

        this.kontejner=document.createElement("div");
        this.kontejner.className="kontejner";
        host.appendChild(this.kontejner);

        let naslovKontejner=document.createElement("div");
        naslovKontejner.className="naslov";
        this.kontejner.appendChild(naslovKontejner);

        let naslovLabela=document.createElement("label");
        naslovLabela.innerHTML=this.naziv;
        naslovLabela.className="naslovLabela"
        naslovKontejner.appendChild(naslovLabela);
 
        let zaradaLabela=document.createElement("label");
        let zarada;

        fetch("https://localhost:5001/Hotel/DnevnaZaradaHotela/"+this.naziv)
        .then(z=>{
            z.json().then(zh=>{
                zarada=zh;
                zaradaLabela.innerHTML="Dnevna zarada: "+zarada;
            });
        });
       
        naslovKontejner.appendChild(zaradaLabela);

        let ostaloKontejner=document.createElement("div");
        ostaloKontejner.className="ostalo";
        this.kontejner.appendChild(ostaloKontejner);

        let meniKontejner=document.createElement("div");
        meniKontejner.className="meniKontejner";
        ostaloKontejner.appendChild(meniKontejner);
        this.prikaziMeni(meniKontejner);

    }

    prikaziMeni(host){

        host.innerHTML="";
        let recepcionerDiv=document.createElement("div");

        let recepcionerLabela=document.createElement("label");
        recepcionerLabela.innerHTML="Recepcioner:";
        host.appendChild(recepcionerDiv);
        recepcionerDiv.appendChild(recepcionerLabela);

        let recepcionerSelect=document.createElement("select");
        recepcionerDiv.appendChild(recepcionerSelect);

        let op;
        this.Recepcioneri.forEach(rec=>{
            op=document.createElement("option");
            op.innerHTML=rec.ime+" "+rec.prezime;
            op.value=rec.id;
            recepcionerSelect.appendChild(op);
        })

        let podaciKontejner=document.createElement("div");
        podaciKontejner.className="podaciKontejner";
        host.appendChild(podaciKontejner);

        let recOkBtn=document.createElement("button");
        recOkBtn.innerHTML="OK";
        recOkBtn.onclick=(ev)=>this.prikaziPodatke(podaciKontejner);
        recepcionerDiv.appendChild(recOkBtn);


        this.prikaziPodatke(podaciKontejner);

        let unosKontejner=document.createElement("div");
        unosKontejner.className="unosKontejner";
        host.appendChild(unosKontejner);

        this.prikaziUnos(unosKontejner);
    }

    prikaziPodatke(host){

        host.innerHTML="";

        let recepcionerSelect=host.parentNode.querySelector("select");
        let recID=recepcionerSelect.options[recepcionerSelect.selectedIndex].value;
        let recepcioner;

        this.Recepcioneri.forEach(r=>{
            if(r.id==recID){
                recepcioner=r;
            }
        });


        let naslovDiv=document.createElement("div");
        naslovDiv.className="naslovLabela";
        naslovDiv.innerHTML="Podaci o recepcioneru";
        host.appendChild(naslovDiv);

        let imeDiv=document.createElement("div");
        imeDiv.className="pomocniKontejner";
        host.appendChild(imeDiv);

        let imeLabel=document.createElement("label");
        imeLabel.className="labelePodataka";
        imeLabel.innerHTML="Ime: ";
        imeDiv.appendChild(imeLabel);

        let imeTbx=document.createElement("input");
        imeTbx.setAttribute("type", "text");
        imeTbx.setAttribute("placeholder", recepcioner.ime);
       // imeTbx.className="tbxPodataka";
        imeDiv.appendChild(imeTbx);

        let prezimeDiv=document.createElement("div");
        prezimeDiv.className="pomocniKontejner";
        host.appendChild(prezimeDiv);

        let prezimeLabel=document.createElement("label");
        prezimeLabel.className="labelePodataka";
        prezimeLabel.innerHTML="Prezime: ";
        prezimeDiv.appendChild(prezimeLabel);

        let prezimeTbx=document.createElement("input");
        prezimeTbx.setAttribute("type", "text");
        prezimeTbx.setAttribute("placeholder", recepcioner.prezime);
      //  prezimeTbx.className="tbxPodataka";
        prezimeDiv.appendChild(prezimeTbx);

        
        let idKarticaDiv=document.createElement("div");
        idKarticaDiv.className="pomocniKontejner";
        host.appendChild(idKarticaDiv);

        let idKarticaLabel=document.createElement("label");
        idKarticaLabel.className="labelePodataka";
        idKarticaLabel.innerHTML="Br. ID kartice: ";
        idKarticaDiv.appendChild(idKarticaLabel);

        let idKarticaTbx=document.createElement("input");
        idKarticaTbx.setAttribute("type", "text");
        idKarticaTbx.setAttribute("placeholder", recepcioner.idKartica);
      //  idKarticaTbx.className="tbxPodataka";
        idKarticaDiv.appendChild(idKarticaTbx);

        let labelDiv=document.createElement("div");
        labelDiv.className="naslov";
        host.appendChild(labelDiv);

        let sobeLabel=document.createElement("label");
        sobeLabel.innerHTML="Izdate sobe";
        labelDiv.appendChild(sobeLabel);

        let sobeDiv=document.createElement("div");
        sobeDiv.className="pomocniKontejner";
        host.appendChild(sobeDiv);

        if(recepcioner.Sobe.length!=0) {

            let listaSoba=document.createElement("ul");
            listaSoba.className="lista";

            recepcioner.Sobe.forEach(s=> {
                let listEl=document.createElement("li");
                listEl.innerHTML=s.broj+"\t"+s.gost.ime.toString()+" "+s.gost.prezime.toString();
                listaSoba.appendChild(listEl);
                });

            sobeDiv.appendChild(listaSoba);
        }
        else{

            let praznoLabel=document.createElement("label");
            praznoLabel.innerHTML="Nema izdatih soba";
            sobeDiv.appendChild(praznoLabel);
        }

        let dugmadDiv=document.createElement("div");
        dugmadDiv.className="dugmad";
        host.appendChild(dugmadDiv);


        let izmeniDugme=document.createElement("button");
        izmeniDugme.innerHTML="Izmeni";
        izmeniDugme.onclick=(ev)=>this.izmeniPodatke(recepcioner, imeTbx, prezimeTbx, idKarticaTbx, host );
        dugmadDiv.appendChild(izmeniDugme);

        let otpustiDugme=document.createElement("button");
        otpustiDugme.innerHTML="Otpusti";
        otpustiDugme.onclick=(ev)=>this.otpustiRecepcionera(recepcioner);
        dugmadDiv.appendChild(otpustiDugme);
        
    }

    prikaziUnos(host){

        let naslovDiv=document.createElement("div");
        naslovDiv.className="naslovLabela";
        naslovDiv.innerHTML="Dodavanje novog recepcionera";
        host.appendChild(naslovDiv);

        let unosDiv=document.createElement("div");
        unosDiv.className="pomocniKontejner";
        host.appendChild(unosDiv);

        let imeDiv=document.createElement("div");
        imeDiv.className="pomocniKontejner";
        unosDiv.appendChild(imeDiv);

        let imeLabel=document.createElement("label");
        imeLabel.className="labelePodataka";
        imeLabel.innerHTML="Ime: ";
        imeDiv.appendChild(imeLabel);

        let imeTbx=document.createElement("input");
        imeTbx.setAttribute("type", "text");
        imeDiv.appendChild(imeTbx);

        let prezimeDiv=document.createElement("div");
        prezimeDiv.className="pomocniKontejner";
        unosDiv.appendChild(prezimeDiv);

        let prezimeLabel=document.createElement("label");
        prezimeLabel.className="labelePodataka";
        prezimeLabel.innerHTML="Prezime: ";
        prezimeDiv.appendChild(prezimeLabel);

        let prezimeTbx=document.createElement("input");
        prezimeTbx.setAttribute("type", "text");
        prezimeDiv.appendChild(prezimeTbx);

        
        let idKarticaDiv=document.createElement("div");
        idKarticaDiv.className="pomocniKontejner";
        unosDiv.appendChild(idKarticaDiv);

        let idKarticaLabel=document.createElement("label");
        idKarticaLabel.className="labelePodataka";
        idKarticaLabel.innerHTML="Br. ID kartice: ";
        idKarticaDiv.appendChild(idKarticaLabel);

        let idKarticaTbx=document.createElement("input");
        idKarticaTbx.setAttribute("type", "text");
        idKarticaDiv.appendChild(idKarticaTbx);

        let okDugme=document.createElement("button");
        okDugme.innerHTML="OK";
        okDugme.onclick=(ev)=>this.zaposliRecepcionera(imeTbx, prezimeTbx, idKarticaTbx);
        host.appendChild(okDugme);
    }

    updatePrikazaPodataka()
    {
        let meniKontejner=this.kontejner.querySelector(".meniKontejner");
        meniKontejner.innerHTML="";
        this.prikaziMeni(meniKontejner);
    }

    izmeniPodatke(recepcioner, imeTbx, prezimeTbx, idKarticaTbx, host ){

        let ime=imeTbx.value;
        let prezime=prezimeTbx.value;
        let idKartica=idKarticaTbx.value;
        let zaIzmenu;

        if(ime==="" && prezime==="" && idKartica=="") return;

        this.Recepcioneri.forEach(r=>{
            if (r.id===recepcioner.id)
                zaIzmenu=r;
        });

        if(idKartica==""||idKartica==null||idKartica==undefined) idKartica=recepcioner.idKartica;//za slucaj da se ne menja id kartica

        if(idKartica.length!=5 || parseInt(idKartica)===NaN) {
            alert("Broj ID kartice treba da se sastoji od tacno 5 cifara!");
            return;
        }

        if(ime!=null && ime!=undefined && ime!=""){
            fetch("https://localhost:5001/Recepcioner/IzmenaImenaRecepcionera/"+idKartica+"/"+ime,
            {
                method:"PUT"
            }).then(s=>{
                if(s.ok){
                    alert("Podaci o recepcioneru su izmenjeni!");
                    zaIzmenu.ime=ime;
                    this.updatePrikazaPodataka(host.parentNode.parentNode);
                }
                else{
                    alert("Doslo je do greske!");
                }
               
            });
            
        }

        if(prezime!=null && prezime!=undefined && prezime!=""){
            fetch("https://localhost:5001/Recepcioner/IzmenaPrezimenaRecepcionera/"+idKartica+"/"+prezime,
            {
                method:"PUT"
            }).then(s=>{
                if(s.ok){
                    alert("Podaci o recepcioneru su izmenjeni!");
                    zaIzmenu.prezime=prezime;
                    this.updatePrikazaPodataka(host.parentNode.parentNode);
                }
                else{
                    alert("Doslo je do greske!");
                }
            })
            
        }

        if(idKarticaTbx.value.length===5 && parseInt(idKarticaTbx.value)!=NaN)
        {
            fetch("https://localhost:5001/Recepcioner/IzmenaBrojaIDKartice/"+recepcioner.idKartica+"/"+idKartica,
            {
                method:"PUT"
            }).then(s=>{
                if(s.ok){
                    alert("Podaci o recepcioneru su izmenjeni!");
                    zaIzmenu.idKartica=idKartica;
                    this.updatePrikazaPodataka(host.parentNode.parentNode);
                }
                else{
                    alert("Doslo je do greske!");
                }
            })
           
        }
        
    }

    otpustiRecepcionera(recepcioner){


        fetch("https://localhost:5001/Recepcioner/UklanjanjeRecepcionera/"+this.naziv+"/"+recepcioner.idKartica,
        {
           method:"DELETE"
        }).then(r=>{
           if(r.ok){//opet ucitavam recepcionere jer je kontroler namesten da random recepcioneru prebaci sobe
            this.Recepcioneri=[];

            fetch("https://localhost:5001/Recepcioner/PreuzimanjeRecepcionera/"+this.naziv)
            .then(r=>{
                r.json().then(recepcioneri=>{
                    recepcioneri.forEach(recep => {
                        let rec=new Recepcioner(recep.recepcionerID, recep.ime, recep.prezime, recep.iD_kartica);
                        recep.izdateSobe.forEach(is=>{
                            id=is.sobaID;
                            this.Sobe.forEach(s=>{
                                if(s.id===id){
                                    
                                    rec.Sobe.push(s);
                                }
                            });
                        });
                        this.Recepcioneri.push(rec); 
                    });
                    this.updatePrikazaPodataka();
                 });
               });
           }
           else{
               alert("Doslo je do greske prilikom brisanja!");
           }
        });
    }

    zaposliRecepcionera(imeTbx, prezimeTbx, idKarticaTbx){

        let ime=imeTbx.value;
        if(ime==="") alert("Nedostaje ime!");

        let prezime=prezimeTbx.value;
        if(prezime==="") alert("Nedostaje prezime!");

        let idKartica=idKarticaTbx.value;
        if(idKartica==="") alert("Nedostaje broj id kartice!");

        if(idKartica.length!=5 || parseInt(idKartica)===NaN) {
            alert("Broj ID kartice treba da se sastoji od tacno 5 cifara!");
            return;
        }

        fetch("https://localhost:5001/Recepcioner/UnosRecepcionera/"+ime+"/"+prezime+"/"+idKartica+"/"+this.naziv,
        {
            method:"POST"
        })
        .then(r=>{
            if(r.ok){
                r.json().then(rc=>{
                    let rec=new Recepcioner(rc.recepcionerID, rc.ime, rc.prezime, rc.iD_kartica);
                    this.Recepcioneri.push(rec);
                    alert("Recepcioner dodat!")

                    this.updatePrikazaPodataka();
                });
            }
        });

    }
}