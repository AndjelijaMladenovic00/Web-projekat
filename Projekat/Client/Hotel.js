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

        fetch("https://localhost:5001/Hotel/DnevnaZaradaHotela/"+this.id)
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

        let prikazKontejner=document.createElement("div");
        prikazKontejner.className="prikazKontejner";
        ostaloKontejner.appendChild(prikazKontejner);
        this.prikaziSobe(prikazKontejner);

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
        });

        let podaciKontejner=document.createElement("div");
        podaciKontejner.className="podaciKontejner";
        host.appendChild(podaciKontejner);

        let recOkBtn=document.createElement("button");
        recOkBtn.innerHTML="OK";
        recOkBtn.onclick=(ev)=>this.prikaziPodatke(podaciKontejner);
        recepcionerDiv.appendChild(recOkBtn);

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

        console.log(recepcioner);

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

        recepcioner.Sobe=[];

        fetch("https://localhost:5001/Soba/PreuzimanjeSobaZaRecepcionera/"+recepcioner.id)
        .then(s=>{
            s.json().then(sobe=>{
                console.log(sobe);
                sobe.forEach(s=>{
                    let soba;
                    this.Sobe.forEach(sb=>{

                        if (sb.id===s.sobaID){
                            let gost=new Gost(s.gost.gostID, s.gost.ime, s.gost.prezime, s.gost.brojLicneKarte);
                            sb.gost=gost;
                            recepcioner.Sobe.push(sb);
                        }
                    });

                    if(recepcioner.Sobe===[]) {

                        let praznoLabel=document.createElement("label");
                        praznoLabel.innerHTML="Nema izdatih soba";
                        sobeDiv.appendChild(praznoLabel);
                        
                    }
                    else {
                        
                        let listaSoba=document.createElement("ul");
                        listaSoba.className="lista";
            
                        recepcioner.Sobe.forEach(s=> {

                            let listEl=document.createElement("li");
                            listEl.innerHTML=s.broj+"\t"+s.gost.ime.toString()+" "+s.gost.prezime.toString();
                            listaSoba.appendChild(listEl);
                            });
            
                        sobeDiv.innerHTML="";
                        sobeDiv.appendChild(listaSoba);
                    }
                });
            });
        });

        

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

        if(ime==="" && prezime==="" && idKartica=="") return; //ako se slucajno pritisne dugme npr


        this.Recepcioneri.forEach(r=>{

            if (r.id===recepcioner.id)
                zaIzmenu=r;
        });

        if(idKartica==""||idKartica==null||idKartica==undefined) idKartica=recepcioner.idKartica;//za slucaj da se ne menja id kartica
        if(ime==""||ime==null||ime==undefined) ime=recepcioner.ime;
        if(prezime==""||prezime==null||prezime==undefined) prezime=recepcioner.prezime;

        if(idKartica.length!=5 || parseInt(idKartica)===NaN) {
            alert("Broj ID kartice treba da se sastoji od tacno 5 cifara!");
            return;
        }

        if(ime.length>50) alert("Ime predugacko!");
        if(prezime.length>50) alert("Prezime predugacko!");

        fetch("https://localhost:5001/Recepcioner/IzmenaPodataka/"+recepcioner.id+"/"+idKartica+"/"+ime+"/"+prezime,
        {
            method:"POST"
        }).then(s=>{
            if(s.ok){
                s.json().then(rec=>{
                    let prepravljeni=new Recepcioner(rec.recepcionerID, rec.ime, rec.prezime, rec.iD_kartica);

                    this.Recepcioneri.forEach(r=>{

                        if(r.id===prepravljeni.id){
                            r=prepravljeni;
                        };
                    });

                    this.updatePrikazaPodataka();
                });
            }
            else{
                alert("Doslo je do greske!")
            }
        });
        
        
    }

    otpustiRecepcionera(recepcioner){


        fetch("https://localhost:5001/Recepcioner/UklanjanjeRecepcionera/"+recepcioner.id,
        {
           method:"DELETE"
        }).then(r=>{
           if(r.ok){
                
                this.Recepcioneri.forEach((r,index)=>{

                    if (r.id===recepcioner.id){
                    this.Recepcioneri.splice(index, 1);
                    };
                });
                alert("Recepcioner je obrisan!");
                this.updatePrikazaPodataka();
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

        let duplikat; 
        this.Recepcioneri.forEach(r=>{

            if(r.idKartica==idKartica){

                alert("Vec postoji recepcioner sa ovim brojem ID kartice!");
                duplikat=true;
                return;
            }
        });

        if(duplikat===true) return;

        fetch("https://localhost:5001/Recepcioner/UnosRecepcionera/"+ime+"/"+prezime+"/"+idKartica+"/"+this.id,
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
            else{
                alert("Greska pri zaposljavanju recepcionera!")
            }
        });

    }

    prikaziSobe(host){

        let spratovi=[];

        let i;
        for(i=1;i<=this.brSpratova;i++){

            let spratKontejner=document.createElement("div");
            spratKontejner.className="spratKontejner";

            let imeSprata=document.createElement("div");
            imeSprata.innerHTML="Sprat br. "+i;
            imeSprata.className="naslov";
            spratKontejner.appendChild(imeSprata);

            let sprat=document.createElement("div");
            sprat.className="sprat";
            spratovi.push(sprat);
            spratKontejner.appendChild(sprat);

            host.appendChild(spratKontejner);
        }

        this.Sobe.sort((a, b)=>a.broj-b.broj);

        this.Sobe.forEach(s=>{

            let brS=Math.ceil(s.broj/this.brSobaPoSpratu);

            let soba=document.createElement("div");

            if(s.izdata==true) {
                soba.className="izdataSoba";
            }
            else{
                soba.className="praznaSoba";
            }

            let br=document.createElement("label");
            br.innerHTML="Br. sobe: "+s.broj;
            soba.appendChild(br);

            let brK=document.createElement("label");
            brK.innerHTML="Br. kreveta: "+s.brKreveta;
            soba.appendChild(brK);

            let kat=document.createElement("label");
            kat.innerHTML="Kategorija: "+s.kategorija;
            soba.appendChild(kat);

            spratovi[brS-1].appendChild(soba);
        });
    }
}