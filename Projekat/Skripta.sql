INSERT INTO Hoteli(HotelID,Naziv,Lokacija,BrojSpratova,BrojSobaPoSpratu,Cena_I_kat,Cena_II_kat,Cena_III_kat) VALUES (1,'Hotel 1','sda',2,4,5,4,3);
INSERT INTO Hoteli(HotelID,Naziv,Lokacija,BrojSpratova,BrojSobaPoSpratu,Cena_I_kat,Cena_II_kat,Cena_III_kat) VALUES (2,'Hotel 2','l',2,3,4,2,1);
INSERT INTO Hoteli(HotelID,Naziv,Lokacija,BrojSpratova,BrojSobaPoSpratu,Cena_I_kat,Cena_II_kat,Cena_III_kat) VALUES (3,'Hotel 3','lok',3,4,5,4,2);

INSERT INTO Recepcioneri(RecepcionerID,Ime,Prezime,ID_kartica,HotelID) VALUES (23,'Igor','Petrovic',00001,1);
INSERT INTO Recepcioneri(RecepcionerID,Ime,Prezime,ID_kartica,HotelID) VALUES (25,'Marko','Krstic',00001,2);
INSERT INTO Recepcioneri(RecepcionerID,Ime,Prezime,ID_kartica,HotelID) VALUES (33,'Ivan','Nikolic',00099,1);
INSERT INTO Recepcioneri(RecepcionerID,Ime,Prezime,ID_kartica,HotelID) VALUES (1033,'Ana','Markovic',00009,1);
INSERT INTO Recepcioneri(RecepcionerID,Ime,Prezime,ID_kartica,HotelID) VALUES (1035,'Milos','Mitrovic',00003,2);
INSERT INTO Recepcioneri(RecepcionerID,Ime,Prezime,ID_kartica,HotelID) VALUES (1036,'Igor','Nenadovic',00023,1);
INSERT INTO Recepcioneri(RecepcionerID,Ime,Prezime,ID_kartica,HotelID) VALUES (1037,'Nikola','Nikolic',00001,3);

INSERT INTO Gosti(GostID,Ime,Prezime,BrojLicneKarte,HotelID) VALUES (1,'Ivan','Nikolic',111111112,1);
INSERT INTO Gosti(GostID,Ime,Prezime,BrojLicneKarte,HotelID) VALUES (1003,'Milica','Igic',020399521,2);
INSERT INTO Gosti(GostID,Ime,Prezime,BrojLicneKarte,HotelID) VALUES (1004,'Jana','Miljkovic',230497723,1);

INSERT INTO Sobe(SobaID,BrojSobe,BrojKreveta,Kategorija,Izdata,HotelID,RecepcionerID,GostID) VALUES (1,3,2,1,0,1,NULL,NULL);
INSERT INTO Sobe(SobaID,BrojSobe,BrojKreveta,Kategorija,Izdata,HotelID,RecepcionerID,GostID) VALUES (2,4,4,2,0,1,NULL,NULL);
INSERT INTO Sobe(SobaID,BrojSobe,BrojKreveta,Kategorija,Izdata,HotelID,RecepcionerID,GostID) VALUES (3,5,2,2,1,1,23,1);
INSERT INTO Sobe(SobaID,BrojSobe,BrojKreveta,Kategorija,Izdata,HotelID,RecepcionerID,GostID) VALUES (1003,6,2,1,1,1,23,1);
INSERT INTO Sobe(SobaID,BrojSobe,BrojKreveta,Kategorija,Izdata,HotelID,RecepcionerID,GostID) VALUES (3003,1,1,2,0,2,NULL,NULL);
INSERT INTO Sobe(SobaID,BrojSobe,BrojKreveta,Kategorija,Izdata,HotelID,RecepcionerID,GostID) VALUES (3004,1,2,2,0,1,NULL,NULL);
INSERT INTO Sobe(SobaID,BrojSobe,BrojKreveta,Kategorija,Izdata,HotelID,RecepcionerID,GostID) VALUES (3005,2,1,1,1,1,1033,1004);
INSERT INTO Sobe(SobaID,BrojSobe,BrojKreveta,Kategorija,Izdata,HotelID,RecepcionerID,GostID) VALUES (3006,7,3,1,0,1,NULL,NULL);
INSERT INTO Sobe(SobaID,BrojSobe,BrojKreveta,Kategorija,Izdata,HotelID,RecepcionerID,GostID) VALUES (3007,8,2,2,0,1,NULL,NULL);
INSERT INTO Sobe(SobaID,BrojSobe,BrojKreveta,Kategorija,Izdata,HotelID,RecepcionerID,GostID) VALUES (3008,2,3,2,0,2,NULL,NULL);
INSERT INTO Sobe(SobaID,BrojSobe,BrojKreveta,Kategorija,Izdata,HotelID,RecepcionerID,GostID) VALUES (3009,3,1,1,0,2,NULL,NULL);
INSERT INTO Sobe(SobaID,BrojSobe,BrojKreveta,Kategorija,Izdata,HotelID,RecepcionerID,GostID) VALUES (3010,4,4,1,0,2,NULL,NULL);
INSERT INTO Sobe(SobaID,BrojSobe,BrojKreveta,Kategorija,Izdata,HotelID,RecepcionerID,GostID) VALUES (3011,5,4,3,0,2,NULL,NULL);
INSERT INTO Sobe(SobaID,BrojSobe,BrojKreveta,Kategorija,Izdata,HotelID,RecepcionerID,GostID) VALUES (3012,6,2,3,1,2,25,1003);
INSERT INTO Sobe(SobaID,BrojSobe,BrojKreveta,Kategorija,Izdata,HotelID,RecepcionerID,GostID) VALUES (3013,1,1,1,0,3,NULL,NULL);
INSERT INTO Sobe(SobaID,BrojSobe,BrojKreveta,Kategorija,Izdata,HotelID,RecepcionerID,GostID) VALUES (3014,2,2,1,0,3,NULL,NULL);
INSERT INTO Sobe(SobaID,BrojSobe,BrojKreveta,Kategorija,Izdata,HotelID,RecepcionerID,GostID) VALUES (3015,3,2,2,0,3,NULL,NULL);
INSERT INTO Sobe(SobaID,BrojSobe,BrojKreveta,Kategorija,Izdata,HotelID,RecepcionerID,GostID) VALUES (3016,4,2,2,0,3,NULL,NULL);
INSERT INTO Sobe(SobaID,BrojSobe,BrojKreveta,Kategorija,Izdata,HotelID,RecepcionerID,GostID) VALUES (3017,5,2,2,0,3,NULL,NULL);
INSERT INTO Sobe(SobaID,BrojSobe,BrojKreveta,Kategorija,Izdata,HotelID,RecepcionerID,GostID) VALUES (3018,6,3,2,0,3,NULL,NULL);
INSERT INTO Sobe(SobaID,BrojSobe,BrojKreveta,Kategorija,Izdata,HotelID,RecepcionerID,GostID) VALUES (3019,7,3,3,0,3,NULL,NULL);
INSERT INTO Sobe(SobaID,BrojSobe,BrojKreveta,Kategorija,Izdata,HotelID,RecepcionerID,GostID) VALUES (3020,8,3,3,0,3,NULL,NULL);
INSERT INTO Sobe(SobaID,BrojSobe,BrojKreveta,Kategorija,Izdata,HotelID,RecepcionerID,GostID) VALUES (3021,9,1,3,0,3,NULL,NULL);
INSERT INTO Sobe(SobaID,BrojSobe,BrojKreveta,Kategorija,Izdata,HotelID,RecepcionerID,GostID) VALUES (3022,10,1,3,0,3,NULL,NULL);
INSERT INTO Sobe(SobaID,BrojSobe,BrojKreveta,Kategorija,Izdata,HotelID,RecepcionerID,GostID) VALUES (3023,11,3,3,0,3,NULL,NULL);
INSERT INTO Sobe(SobaID,BrojSobe,BrojKreveta,Kategorija,Izdata,HotelID,RecepcionerID,GostID) VALUES (3024,12,3,3,0,3,NULL,NULL);
