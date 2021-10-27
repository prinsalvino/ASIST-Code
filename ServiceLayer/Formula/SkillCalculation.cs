using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace ServiceLayer.Formula
{
    public class SkillCalculation
    {

        public double[] SkillsCalculation(Gender GESLACHT, double LFT, double Ttijgeren, double Tspringen, double Tbalvaardigheid,
            double Trollen, double Tbehendigheid)
        {
			var TTOTAAL = Ttijgeren + Tspringen + Tbalvaardigheid + Trollen + Tbehendigheid;

			var BLMAN6 = 50;
			var BLMAN7 = 50;
			var BLMAN8 = 40;
			var BLMAN9 = 40;
			var BLMAN10 = 30;
			var BLMAN11 = 30;
			var BLMAN12 = 25;
			var BLMAN13 = 25;

			/*Grenzen oranje mannen*/
			var ORMAN6 = 60;
			var ORMAN7 = 60;
			var ORMAN8 = 55;
			var ORMAN9 = 55;
			var ORMAN10 = 45;
			var ORMAN11 = 45;
			var ORMAN12 = 40;
			var ORMAN13 = 40;

			/*Grenzen maximale tijd mannen*/
			var MAXMAN6 = 70;
			var MAXMAN7 = 70;
			var MAXMAN8 = 65;
			var MAXMAN9 = 65;
			var MAXMAN10 = 55;
			var MAXMAN11 = 55;
			var MAXMAN12 = 45;
			var MAXMAN13 = 45;

			/*Grenzen minimale tijd mannen*/
			var MINMAN6 = 40;
			var MINMAN7 = 40;
			var MINMAN8 = 30;
			var MINMAN9 = 30;
			var MINMAN10 = 20;
			var MINMAN11 = 20;
			var MINMAN12 = 20;
			var MINMAN13 = 20;

			/*Grenzen blauw vrouwen*/
			var BLVRO6 = 50;
			var BLVRO7 = 50;
			var BLVRO8 = 40;
			var BLVRO9 = 40;
			var BLVRO10 = 35;
			var BLVRO11 = 35;
			var BLVRO12 = 30;
			var BLVRO13 = 30;

			/*Grenzen oranje vrouwen*/
			var ORVRO6 = 60;
			var ORVRO7 = 60;
			var ORVRO8 = 55;
			var ORVRO9 = 55;
			var ORVRO10 = 50;
			var ORVRO11 = 50;
			var ORVRO12 = 45;
			var ORVRO13 = 45;

			/*Grenzen maximale tijd vrouwen*/
			var MAXVRO6 = 70;
			var MAXVRO7 = 70;
			var MAXVRO8 = 65;
			var MAXVRO9 = 65;
			var MAXVRO10 = 60;
			var MAXVRO11 = 60;
			var MAXVRO12 = 60;
			var MAXVRO13 = 60;

			/*Grenzen minimale tijd vrouwen*/
			var MINVRO6 = 40;
			var MINVRO7 = 40;
			var MINVRO8 = 30;
			var MINVRO9 = 30;
			var MINVRO10 = 25;
			var MINVRO11 = 25;
			var MINVRO12 = 25;
			var MINVRO13 = 25;


			/*Geslacht omzetten naar cijfer*/
			double GES = new double();
			if (GESLACHT == Gender.Male)
			{
				GES = 1;
			}
			else
			{
				GES = 2;
			}

			/*NULTIJD bepalen*/
			double NULTIJD = new double();
			if (GES == 1 && LFT == 6)
			{
				NULTIJD = MAXMAN6;
			}
			else if (GES == 1 && LFT == 7)
			{
				NULTIJD = MAXMAN7;
			}

			else if (GES == 1 && LFT == 8)
			{
				NULTIJD = MAXMAN8;
			}

			else if (GES == 1 && LFT == 9)
			{
				NULTIJD = MAXMAN9;
			}

			else if (GES == 1 && LFT == 10)
			{
				NULTIJD = MAXMAN10;
			}

			else if (GES == 1 && LFT == 11)
			{
				NULTIJD = MAXMAN11;
			}

			else if (GES == 1 && LFT == 12)
			{
				NULTIJD = MAXMAN12;
			}

			else if (GES == 1 && LFT == 13)
			{
				NULTIJD = MAXMAN13;
			}

			else if (GES == 2 && LFT == 6)
			{
				NULTIJD = MAXVRO6;
			}

			else if (GES == 2 && LFT == 7)
			{
				NULTIJD = MAXVRO7;
			}

			else if (GES == 2 && LFT == 8)
			{
				NULTIJD = MAXVRO8;
			}

			else if (GES == 2 && LFT == 9)
			{
				NULTIJD = MAXVRO9;
			}

			else if (GES == 2 && LFT == 10)
			{
				NULTIJD = MAXVRO10;
			}

			else if (GES == 2 && LFT == 11)
			{
				NULTIJD = MAXVRO11;
			}

			else if (GES == 2 && LFT == 12)
			{
				NULTIJD = MAXVRO12;
			}

			else if (GES == 2 && LFT == 13)
			{
				NULTIJD = MAXVRO13;
			}


			/*HONDERDTIJD bepalen*/
			double HONDERDTIJD = new double();
			if (GES == 1 && LFT == 6)
			{
				HONDERDTIJD = MINMAN6;
			}
			else if (GES == 1 && LFT == 7)
			{
				HONDERDTIJD = MINMAN7;
			}

			else if (GES == 1 && LFT == 8)
			{
				HONDERDTIJD = MINMAN8;
			}

			else if (GES == 1 && LFT == 9)
			{
				HONDERDTIJD = MINMAN9;
			}
			else if (GES == 1 && LFT == 10)
			{
				HONDERDTIJD = MINMAN10;
			}

			else if (GES == 1 && LFT == 11)
			{
				HONDERDTIJD = MINMAN11;
			}

			else if (GES == 1 && LFT == 12)
			{
				HONDERDTIJD = MINMAN12;
			}

			else if (GES == 1 && LFT == 13)
			{
				HONDERDTIJD = MINMAN13;
			}

			else if (GES == 2 && LFT == 6)
			{
				HONDERDTIJD = MINVRO6;
			}

			else if (GES == 2 && LFT == 7)
			{
				HONDERDTIJD = MINVRO7;
			}

			else if (GES == 2 && LFT == 8)
			{
				HONDERDTIJD = MINVRO8;
			}

			else if (GES == 2 && LFT == 9)
			{
				HONDERDTIJD = MINVRO9;
			}

			else if (GES == 2 && LFT == 10)
			{
				HONDERDTIJD = MINVRO10;
			}

			else if (GES == 2 && LFT == 11)
			{
				HONDERDTIJD = MINVRO11;
			}

			else if (GES == 2 && LFT == 12)
			{
				HONDERDTIJD = MINVRO12;
			}

			else if (GES == 2 && LFT == 13)
			{
				HONDERDTIJD = MINVRO13;
			}


			/*Verschil in tijd met snelste*/
			double AFSTANDSNELSTE = TTOTAAL - HONDERDTIJD;

			/*Percentage kind bepalen*/

			if (TTOTAAL < HONDERDTIJD)
			{
				double POSITIE = 100;
			}

			else if (TTOTAAL > NULTIJD)
			{
				double POSITIE = 0;
			}

			else
			{
				double POSITIE = 100 - ((AFSTANDSNELSTE / (NULTIJD - HONDERDTIJD)) * 100);
			}


			/*Grensblauw bepalen*/
			double GRENSBLAUW = new double();
			if (GES == 1 && LFT == 6)
			{
				GRENSBLAUW = BLMAN6;
			}
			else if (GES == 1 && LFT == 7)
			{
				GRENSBLAUW = BLMAN7;
			}

			else if (GES == 1 && LFT == 8)
			{
				GRENSBLAUW = BLMAN8;
			}

			else if (GES == 1 && LFT == 9)
			{
				GRENSBLAUW = BLMAN9;
			}

			else if (GES == 1 && LFT == 10)
			{
				GRENSBLAUW = BLMAN10;
			}

			else if (GES == 1 && LFT == 11)
			{
				GRENSBLAUW = BLMAN11;
			}

			else if (GES == 1 && LFT == 12)
			{
				GRENSBLAUW = BLMAN12;
			}

			else if (GES == 1 && LFT == 13)
			{
				GRENSBLAUW = BLMAN13;
			}

			else if (GES == 2 && LFT == 6)
			{
				GRENSBLAUW = BLVRO6;
			}

			else if (GES == 2 && LFT == 7)
			{
				GRENSBLAUW = BLVRO7;
			}

			else if (GES == 2 && LFT == 8)
			{
				GRENSBLAUW = BLVRO8;
			}

			else if (GES == 2 && LFT == 9)
			{
				GRENSBLAUW = BLVRO9;
			}

			else if (GES == 2 && LFT == 10)
			{
				GRENSBLAUW = BLVRO10;
			}

			else if (GES == 2 && LFT == 11)
			{
				GRENSBLAUW = BLVRO11;
			}

			else if (GES == 2 && LFT == 12)
			{
				GRENSBLAUW = BLVRO12;
			}

			else if (GES == 2 && LFT == 13)
			{
				GRENSBLAUW = BLVRO13;
			}

			/*Grensoranje bepalen*/
			double GRENSORANJE = new double();
			if (GES == 1 && LFT == 6)
			{
				GRENSORANJE = ORMAN6;
			}
			else if (GES == 1 && LFT == 7)
			{
				GRENSORANJE = ORMAN7;
			}

			else if (GES == 1 && LFT == 8)
			{
				GRENSORANJE = ORMAN8;
			}

			else if (GES == 1 && LFT == 9)
			{
				GRENSORANJE = ORMAN9;
			}

			else if (GES == 1 && LFT == 10)
			{
				GRENSORANJE = ORMAN10;
			}

			else if (GES == 1 && LFT == 11)
			{
				GRENSORANJE = ORMAN11;
			}

			else if (GES == 1 && LFT == 12)
			{
				GRENSORANJE = ORMAN12;
			}

			else if (GES == 1 && LFT == 13)
			{
				GRENSORANJE = ORMAN13;
			}

			else if (GES == 2 && LFT == 6)
			{
				GRENSORANJE = ORVRO6;
			}

			else if (GES == 2 && LFT == 7)
			{
				GRENSORANJE = ORVRO7;
			}

			else if (GES == 2 && LFT == 8)
			{
				GRENSORANJE = ORVRO8;
			}

			else if (GES == 2 && LFT == 9)
			{
				GRENSORANJE = ORVRO9;
			}

			else if (GES == 2 && LFT == 10)
			{
				GRENSORANJE = ORVRO10;
			}

			else if (GES == 2 && LFT == 11)
			{
				GRENSORANJE = ORVRO11;
			}

			else if (GES == 2 && LFT == 12)
			{
				GRENSORANJE = ORVRO12;
			}

			else if (GES == 2 && LFT == 13)
			{
				GRENSORANJE = ORVRO13;
			}



			/*--> Tijgeren <--*/

			/*Blauw tijd voor tijgeren bij vrouwen*/
			double Tblauwvrouw6 = 5.78;
			double Tblauwvrouw7 = 5.78;
			double Tblauwvrouw8 = 4.9;
			double Tblauwvrouw9 = 5;
			double Tblauwvrouw10 = 4.3;
			double Tblauwvrouw11 = 4.3;
			double Tblauwvrouw12 = 3.8;
			double Tblauwvrouw13 = 3.8;

			/*Oranje tijd voo; tijgeren bij vrouwe;*/
			double Toranjevrouw6 = 11.94;
			double Toranjevrouw7 = 11.94;
			double Toranjevrouw8 = 8.5;
			double Toranjevrouw9 = 8.6;
			double Toranjevrouw10 = 7.8;
			double Toranjevrouw11 = 7.6;
			double Toranjevrouw12 = 6.6;
			double Toranjevrouw13 = 6.6;

			/*Maximale tijd voo; tijgeren bij vrouwe;*/
			double Tmaxvrouw6 = 14.5;
			double Tmaxvrouw7 = 14.35;
			double Tmaxvrouw8 = 11;
			double Tmaxvrouw9 = 10.85;
			double Tmaxvrouw10 = 10.3;
			double Tmaxvrouw11 = 10.3;
			double Tmaxvrouw12 = 8.5;
			double Tmaxvrouw13 = 8.5;

			/*Minimale tijd voo; tijgeren bij vrouwe;*/
			double Tminvrouw6 = 3.5;
			double Tminvrouw7 = 3.35;
			double Tminvrouw8 = 3.2;
			double Tminvrouw9 = 3.05;
			double Tminvrouw10 = 2.6;
			double Tminvrouw11 = 2.5;
			double Tminvrouw12 = 2.5;
			double Tminvrouw13 = 2.4;

			/*Blauw tijd voo; tijgeren bij manne;*/
			double Tblauwman6 = 5.2;
			double Tblauwman7 = 5.2;
			double Tblauwman8 = 4.9;
			double Tblauwman9 = 4.5;
			double Tblauwman10 = 4.1;
			double Tblauwman11 = 3.8;
			double Tblauwman12 = 3.2;
			double Tblauwman13 = 3.2;

			/*Oranje tijd voo; tijgeren bij manne;*/
			double Toranjeman6 = 9;
			double Toranjeman7 = 9;
			double Toranjeman8 = 8.3;
			double Toranjeman9 = 8.4;
			double Toranjeman10 = 7.4;
			double Toranjeman11 = 7.2;
			double Toranjeman12 = 6.6;
			double Toranjeman13 = 6.6;

			/*Maximale tijd voo; tijgeren bij manne;*/
			double Tmaxman6 = 11;
			double Tmaxman7 = 10.9;
			double Tmaxman8 = 10.2;
			double Tmaxman9 = 10.2;
			double Tmaxman10 = 9.1;
			double Tmaxman11 = 9;
			double Tmaxman12 = 8.05;
			double Tmaxman13 = 8;

			/*Minimale tijd voo; tijgeren bij manne;*/
			double Tminman6 = 3.5;
			double Tminman7 = 3.4;
			double Tminman8 = 3.2;
			double Tminman9 = 3.3;
			double Tminman10 = 2.7;
			double Tminman11 = 2.4;
			double Tminman12 = 2.05;
			double Tminman13 = 2;

			;
			double Tijgeren0 = new double();
			if (GES == 1 && LFT == 6)
			{
				Tijgeren0 = Tmaxman6;
			}
			else if (GES == 1 && LFT == 7)
			{
				Tijgeren0 = Tmaxman7;
			}

			else if (GES == 1 && LFT == 8)
			{
				Tijgeren0 = Tmaxman8;
			}

			else if (GES == 1 && LFT == 9)
			{
				Tijgeren0 = Tmaxman9;
			}

			else if (GES == 1 && LFT == 10)
			{
				Tijgeren0 = Tmaxman10;
			}

			else if (GES == 1 && LFT == 11)
			{
				Tijgeren0 = Tmaxman11;
			}

			else if (GES == 1 && LFT == 12)
			{
				Tijgeren0 = Tmaxman12;
			}

			else if (GES == 1 && LFT == 13)
			{
				Tijgeren0 = Tmaxman13;
			}

			else if (GES == 2 && LFT == 6)
			{
				Tijgeren0 = Tmaxvrouw6;
			}

			else if (GES == 2 && LFT == 7)
			{
				Tijgeren0 = Tmaxvrouw7;
			}

			else if (GES == 2 && LFT == 8)
			{
				Tijgeren0 = Tmaxvrouw8;
			}

			else if (GES == 2 && LFT == 9)
			{
				Tijgeren0 = Tmaxvrouw9;
			}

			else if (GES == 2 && LFT == 10)
			{
				Tijgeren0 = Tmaxvrouw10;
			}

			else if (GES == 2 && LFT == 11)
			{
				Tijgeren0 = Tmaxvrouw11;
			}

			else if (GES == 2 && LFT == 12)
			{
				Tijgeren0 = Tmaxvrouw12;
			}

			else if (GES == 2 && LFT == 13)
			{
				Tijgeren0 = Tmaxvrouw13;
			}

			/*Honderdtijd tijgeren bepalen*/
			double Tijgeren100 = new double();
			if (GES == 1 && LFT == 6)
			{
				Tijgeren100 = Tminman6;
			}
			else if (GES == 1 && LFT == 7)
			{
				Tijgeren100 = Tminman7;
			}

			else if (GES == 1 && LFT == 8)
			{
				Tijgeren100 = Tminman8;
			}

			else if (GES == 1 && LFT == 9)
			{
				Tijgeren100 = Tminman9;
			}

			else if (GES == 1 && LFT == 10)
			{
				Tijgeren100 = Tminman10;
			}

			else if (GES == 1 && LFT == 11)
			{
				Tijgeren100 = Tminman11;
			}

			else if (GES == 1 && LFT == 12)
			{
				Tijgeren100 = Tminman12;
			}

			else if (GES == 1 && LFT == 13)
			{
				Tijgeren100 = Tminman13;
			}

			else if (GES == 2 && LFT == 6)
			{
				Tijgeren100 = Tminvrouw6;
			}

			else if (GES == 2 && LFT == 7)
			{
				Tijgeren100 = Tminvrouw7;
			}

			else if (GES == 2 && LFT == 8)
			{
				Tijgeren100 = Tminvrouw8;
			}

			else if (GES == 2 && LFT == 9)
			{
				Tijgeren100 = Tminvrouw9;
			}

			else if (GES == 2 && LFT == 10)
			{
				Tijgeren100 = Tminvrouw10;
			}

			else if (GES == 2 && LFT == 11)
			{
				Tijgeren100 = Tminvrouw11;
			}

			else if (GES == 2 && LFT == 12)
			{
				Tijgeren100 = Tminvrouw12;
			}

			else if (GES == 2 && LFT == 13)
			{
				Tijgeren100 = Tminvrouw13;
			}

			/*Verschil in tijd met snelste*/
			double Tafstandsnelste = Ttijgeren - Tijgeren100;
			double Tpositie = new double();
			/*Percentage kind bepalen*/
			if (Ttijgeren < Tijgeren100)
			{
				Tpositie = 100;
			}

			else if (Ttijgeren > Tijgeren0)
			{
				Tpositie = 0;
			}

			else
			{
				Tpositie = 100 - ((Tafstandsnelste / (Tijgeren0 - Tijgeren100)) * 100);
			}

			/*Blauwe grens tijgeren bepalen*/
			double Tblauw;
			if (GES == 1 && LFT == 6)
			{
				Tblauw = Tblauwman6;
			}
			else if (GES == 1 && LFT == 7)
			{
				Tblauw = Tblauwman7;
			}

			else if (GES == 1 && LFT == 8)
			{
				Tblauw = Tblauwman8;
			}

			else if (GES == 1 && LFT == 9)
			{
				Tblauw = Tblauwman9;
			}

			else if (GES == 1 && LFT == 10)
			{
				Tblauw = Tblauwman10;
			}

			else if (GES == 1 && LFT == 11)
			{
				Tblauw = Tblauwman11;
			}

			else if (GES == 1 && LFT == 12)
			{
				Tblauw = Tblauwman12;
			}

			else if (GES == 1 && LFT == 13)
			{
				Tblauw = Tblauwman13;
			}

			else if (GES == 2 && LFT == 6)
			{
				Tblauw = Tblauwvrouw6;
			}

			else if (GES == 2 && LFT == 7)
			{
				Tblauw = Tblauwvrouw7;
			}

			else if (GES == 2 && LFT == 8)
			{
				Tblauw = Tblauwvrouw8;
			}

			else if (GES == 2 && LFT == 9)
			{
				Tblauw = Tblauwvrouw9;
			}

			else if (GES == 2 && LFT == 10)
			{
				Tblauw = Tblauwvrouw10;
			}

			else if (GES == 2 && LFT == 11)
			{
				Tblauw = Tblauwvrouw11;
			}

			else if (GES == 2 && LFT == 12)
			{
				Tblauw = Tblauwvrouw12;
			}

			else if (GES == 2 && LFT == 13)
			{
				Tblauw = Tblauwvrouw13;
			}

			/*Oranje grens tijgeren bepalen*/
			double Toranje;
			if (GES == 1 && LFT == 6)
			{
				Toranje = Toranjeman6;
			}
			else if (GES == 1 && LFT == 7)
			{
				Toranje = Toranjeman7;
			}

			else if (GES == 1 && LFT == 8)
			{
				Toranje = Toranjeman8;
			}

			else if (GES == 1 && LFT == 9)
			{
				Toranje = Toranjeman9;
			}

			else if (GES == 1 && LFT == 10)
			{
				Toranje = Toranjeman10;
			}

			else if (GES == 1 && LFT == 11)
			{
				Toranje = Toranjeman11;
			}

			else if (GES == 1 && LFT == 12)
			{
				Toranje = Toranjeman12;
			}

			else if (GES == 1 && LFT == 13)
			{
				Toranje = Toranjeman13;
			}

			else if (GES == 2 && LFT == 6)
			{
				Toranje = Toranjevrouw6;
			}

			else if (GES == 2 && LFT == 7)
			{
				Toranje = Toranjevrouw7;
			}

			else if (GES == 2 && LFT == 8)
			{
				Toranje = Toranjevrouw8;
			}

			else if (GES == 2 && LFT == 9)
			{
				Toranje = Toranjevrouw9;
			}

			else if (GES == 2 && LFT == 10)
			{
				Toranje = Toranjevrouw10;
			}

			else if (GES == 2 && LFT == 11)
			{
				Toranje = Toranjevrouw11;
			}

			else if (GES == 2 && LFT == 12)
			{
				Toranje = Toranjevrouw12;
			}

			else if (GES == 2 && LFT == 13)
			{
				Toranje = Toranjevrouw13;
			}


			/*--> Springen <--*/

			/*Blauw tijd voor springen bij vrouwen*/
			double Sblauwvrouw6 = 8.1;
			double Sblauwvrouw7 = 8.1;
			double Sblauwvrouw8 = 6.5;
			double Sblauwvrouw9 = 6.4;
			double Sblauwvrouw10 = 5.7;
			double Sblauwvrouw11 = 5.6;
			double Sblauwvrouw12 = 5;
			double Sblauwvrouw13 = 5;
			/*Oranje tijd voor springen bij vrouwen*/
			double Soranjevrouw6 = 11.9;
			double Soranjevrouw7 = 11.9;
			double Soranjevrouw8 = 9.5;
			double Soranjevrouw9 = 9.5;
			double Soranjevrouw10 = 8.6;
			double Soranjevrouw11 = 8.4;
			double Soranjevrouw12 = 7.8;
			double Soranjevrouw13 = 7.8;
			/*Maximale tijd voor springen bij vrouwen*/
			double Smaxvrouw6 = 14;
			double Smaxvrouw7 = 13.9;
			double Smaxvrouw8 = 11.2;
			double Smaxvrouw9 = 11;
			double Smaxvrouw10 = 10.2;
			double Smaxvrouw11 = 9.8;
			double Smaxvrouw12 = 9.3;
			double Smaxvrouw13 = 9.25;
			/*Minimale tijd voor springen bij vrouwen*/
			double Sminvrouw6 = 6.3;
			double Sminvrouw7 = 6.25;
			double Sminvrouw8 = 5.1;
			double Sminvrouw9 = 5;
			double Sminvrouw10 = 4.3;
			double Sminvrouw11 = 4.2;
			double Sminvrouw12 = 3.8;
			double Sminvrouw13 = 3.75;
			/*Blauw tijd voor springen bij mannen*/
			double Sblauwman6 = 7.3;
			double Sblauwman7 = 7.3;
			double Sblauwman8 = 6.3;
			double Sblauwman9 = 5.9;
			double Sblauwman10 = 5.1;
			double Sblauwman11 = 5;
			double Sblauwman12 = 4.4;
			double Sblauwman13 = 4.4;
			/*Oranje tijd voor springen bij mannen*/
			double Soranjeman6 = 10.3;
			double Soranjeman7 = 10.3;
			double Soranjeman8 = 9.7;
			double Soranjeman9 = 9.2;
			double Soranjeman10 = 8.2;
			double Soranjeman11 = 7.7;
			double Soranjeman12 = 7.6;
			double Soranjeman13 = 7.6;
			/*Maximale tijd voor springen bij mannen*/
			double Smaxman6 = 12;
			double Smaxman7 = 11.95;
			double Smaxman8 = 11.4;
			double Smaxman9 = 11;
			double Smaxman10 = 9.7;
			double Smaxman11 = 9.1;
			double Smaxman12 = 9.1;
			double Smaxman13 = 9.1;
			/*Minimale tijd voor springen bij mannen*/
			double Sminman6 = 5.9;
			double Sminman7 = 5.85;
			double Sminman8 = 4.9;
			double Sminman9 = 4.5;
			double Sminman10 = 3.8;
			double Sminman11 = 3.6;
			double Sminman12 = 3.15;
			double Sminman13 = 3.1;


			/*Nultijd springen bepalen*/
			double Springen0 = new double();
			if (GES == 1 && LFT == 6)
			{
				Springen0 = Smaxman6;
			}
			else if (GES == 1 && LFT == 7)
			{
				Springen0 = Smaxman7;
			}

			else if (GES == 1 && LFT == 8)
			{
				Springen0 = Smaxman8;
			}

			else if (GES == 1 && LFT == 9)
			{
				Springen0 = Smaxman9;
			}

			else if (GES == 1 && LFT == 10)
			{
				Springen0 = Smaxman10;
			}

			else if (GES == 1 && LFT == 11)
			{
				Springen0 = Smaxman11;
			}

			else if (GES == 1 && LFT == 12)
			{
				Springen0 = Smaxman12;
			}

			else if (GES == 1 && LFT == 13)
			{
				Springen0 = Smaxman13;
			}

			else if (GES == 2 && LFT == 6)
			{
				Springen0 = Smaxvrouw6;
			}

			else if (GES == 2 && LFT == 7)
			{
				Springen0 = Smaxvrouw7;
			}

			else if (GES == 2 && LFT == 8)
			{
				Springen0 = Smaxvrouw8;
			}

			else if (GES == 2 && LFT == 9)
			{
				Springen0 = Smaxvrouw9;
			}

			else if (GES == 2 && LFT == 10)
			{
				Springen0 = Smaxvrouw10;
			}

			else if (GES == 2 && LFT == 11)
			{
				Springen0 = Smaxvrouw11;
			}

			else if (GES == 2 && LFT == 12)
			{
				Springen0 = Smaxvrouw12;
			}

			else if (GES == 2 && LFT == 13)
			{
				Springen0 = Smaxvrouw13;
			}

			/*Honderdtijd springen bepalen*/
			double Springen100 = new double();
			if (GES == 1 && LFT == 6)
			{
				Springen100 = Sminman6;
			}
			else if (GES == 1 && LFT == 7)
			{
				Springen100 = Sminman7;
			}

			else if (GES == 1 && LFT == 8)
			{
				Springen100 = Sminman8;
			}

			else if (GES == 1 && LFT == 9)
			{
				Springen100 = Sminman9;
			}

			else if (GES == 1 && LFT == 10)
			{
				Springen100 = Sminman10;
			}

			else if (GES == 1 && LFT == 11)
			{
				Springen100 = Sminman11;
			}

			else if (GES == 1 && LFT == 12)
			{
				Springen100 = Sminman12;
			}

			else if (GES == 1 && LFT == 13)
			{
				Springen100 = Sminman13;
			}

			else if (GES == 2 && LFT == 6)
			{
				Springen100 = Sminvrouw6;
			}

			else if (GES == 2 && LFT == 7)
			{
				Springen100 = Sminvrouw7;
			}

			else if (GES == 2 && LFT == 8)
			{
				Springen100 = Sminvrouw8;
			}

			else if (GES == 2 && LFT == 9)
			{
				Springen100 = Sminvrouw9;
			}

			else if (GES == 2 && LFT == 10)
			{
				Springen100 = Sminvrouw10;
			}

			else if (GES == 2 && LFT == 11)
			{
				Springen100 = Sminvrouw11;
			}

			else if (GES == 2 && LFT == 12)
			{
				Springen100 = Sminvrouw12;
			}

			else if (GES == 2 && LFT == 13)
			{
				Springen100 = Sminvrouw13;
			}

			/*Verschil in tijd met snelste*/
			double Safstandsnelste = Tspringen - Springen100;

			double Spositie = new double();
			/*Percentage kind bepalen*/
			if (Tspringen < Springen100)
			{
				Spositie = 100;
			}

			else if (Tspringen > Springen0)
			{
				Spositie = 0;
			}

			else
			{
				Spositie = 100 - ((Safstandsnelste / (Springen0 - Springen100)) * 100);
			}

			/*Blauwe grens springen bepalen*/
			double Sblauw;
			if (GES == 1 && LFT == 6)
			{
				Sblauw = Sblauwman6;
			}
			else if (GES == 1 && LFT == 7)
			{
				Sblauw = Sblauwman7;
			}

			else if (GES == 1 && LFT == 8)
			{
				Sblauw = Sblauwman8;
			}

			else if (GES == 1 && LFT == 9)
			{
				Sblauw = Sblauwman9;
			}

			else if (GES == 1 && LFT == 10)
			{
				Sblauw = Sblauwman10;
			}

			else if (GES == 1 && LFT == 11)
			{
				Sblauw = Sblauwman11;
			}

			else if (GES == 1 && LFT == 12)
			{
				Sblauw = Sblauwman12;
			}

			else if (GES == 1 && LFT == 13)
			{
				Sblauw = Sblauwman13;
			}

			else if (GES == 2 && LFT == 6)
			{
				Sblauw = Sblauwvrouw6;
			}

			else if (GES == 2 && LFT == 7)
			{
				Sblauw = Sblauwvrouw7;
			}

			else if (GES == 2 && LFT == 8)
			{
				Sblauw = Sblauwvrouw8;
			}

			else if (GES == 2 && LFT == 9)
			{
				Sblauw = Sblauwvrouw9;
			}

			else if (GES == 2 && LFT == 10)
			{
				Sblauw = Sblauwvrouw10;
			}

			else if (GES == 2 && LFT == 11)
			{
				Sblauw = Sblauwvrouw11;
			}

			else if (GES == 2 && LFT == 12)
			{
				Sblauw = Sblauwvrouw12;
			}

			else if (GES == 2 && LFT == 13)
			{
				Sblauw = Sblauwvrouw13;
			}

			/*Oranje grens springen bepalen*/
			double Soranje;
			if (GES == 1 && LFT == 6)
			{
				Soranje = Soranjeman6;
			}
			else if (GES == 1 && LFT == 7)
			{
				Soranje = Soranjeman7;
			}

			else if (GES == 1 && LFT == 8)
			{
				Soranje = Soranjeman8;
			}

			else if (GES == 1 && LFT == 9)
			{
				Soranje = Soranjeman9;
			}

			else if (GES == 1 && LFT == 10)
			{
				Soranje = Soranjeman10;
			}

			else if (GES == 1 && LFT == 11)
			{
				Soranje = Soranjeman11;
			}

			else if (GES == 1 && LFT == 12)
			{
				Soranje = Soranjeman12;
			}

			else if (GES == 1 && LFT == 13)
			{
				Soranje = Soranjeman13;
			}

			else if (GES == 2 && LFT == 6)
			{
				Soranje = Soranjevrouw6;
			}

			else if (GES == 2 && LFT == 7)
			{
				Soranje = Soranjevrouw7;
			}

			else if (GES == 2 && LFT == 8)
			{
				Soranje = Soranjevrouw8;
			}

			else if (GES == 2 && LFT == 9)
			{
				Soranje = Soranjevrouw9;
			}

			else if (GES == 2 && LFT == 10)
			{
				Soranje = Soranjevrouw10;
			}

			else if (GES == 2 && LFT == 11)
			{
				Soranje = Soranjevrouw11;
			}

			else if (GES == 2 && LFT == 12)
			{
				Soranje = Soranjevrouw12;
			}

			else if (GES == 2 && LFT == 13)
			{
				Soranje = Soranjevrouw13;
			}



			/*--> Balvaardigheid <--*/

			/*Blauw tijd voor balvaardigheid bij vrouwen*/
			double BAblauwvrouw6 = 14.7f,
				BAblauwvrouw7 = 14.7f,
				BAblauwvrouw8 = 12,
				BAblauwvrouw9 = 10.9f,
				BAblauwvrouw10 = 10,
				BAblauwvrouw11 = 9.4f,
				BAblauwvrouw12 = 8.7f,
				BAblauwvrouw13 = 8.7f,

				/*Oranje tijd voor balvaardigheid bij vrouwen*/
				BAoranjevrouw6 = 20.1f,
				BAoranjevrouw7 = 20.1f,
				BAoranjevrouw8 = 18,
				BAoranjevrouw9 = 17,
				BAoranjevrouw10 = 15,
				BAoranjevrouw11 = 14.9f,
				BAoranjevrouw12 = 12.9f,
				BAoranjevrouw13 = 12.9f,

				/*Maximale tijd voor balvaardigheid bij vrouwen*/
				BAmaxvrouw6 = 23,
				BAmaxvrouw7 = 22.95f,
				BAmaxvrouw8 = 21,
				BAmaxvrouw9 = 20,
				BAmaxvrouw10 = 17.5f,
				BAmaxvrouw11 = 17.5f,
				BAmaxvrouw12 = 15.1f,
				BAmaxvrouw13 = 15.05f,

				/*Minimale tijd voor balvaardigheid bij vrouwen*/
				BAminvrouw6 = 11.5f,
				BAminvrouw7 = 11.45f,
				BAminvrouw8 = 9,
				BAminvrouw9 = 8.4f,
				BAminvrouw10 = 7.5f,
				BAminvrouw11 = 7.1f,
				BAminvrouw12 = 6.8f,
				BAminvrouw13 = 6.75f,

				/*Blauw tijd voor balvaardigheid bij mannen*/
				BAblauwman6 = 13.7f,
				BAblauwman7 = 13.7f,
				BAblauwman8 = 10.8f,
				BAblauwman9 = 10.5f,
				BAblauwman10 = 8.7f,
				BAblauwman11 = 8.7f,
				BAblauwman12 = 7.5f,
				BAblauwman13 = 7.5f,

				/*Oranje tijd voor balvaardigheid bij mannen*/
				BAoranjeman6 = 22,
				BAoranjeman7 = 22,
				BAoranjeman8 = 18.8f,
				BAoranjeman9 = 17.4f,
				BAoranjeman10 = 13.8f,
				BAoranjeman11 = 13.6f,
				BAoranjeman12 = 14,
				BAoranjeman13 = 14,

				/*Maximale tijd voor balvaardigheid bij mannen*/
				BAmaxman6 = 26.6f,
				BAmaxman7 = 26.55f,
				BAmaxman8 = 22.8f,
				BAmaxman9 = 21.3f,
				BAmaxman10 = 16.3f,
				BAmaxman11 = 16.25f,
				BAmaxman12 = 16.7f,
				BAmaxman13 = 16.65f,

				/*Minimale tijd voor balvaardigheid bij mannen*/
				BAminman6 = 9.5f,
				BAminman7 = 9.45f,
				BAminman8 = 6.8f,
				BAminman9 = 6.8f,
				BAminman10 = 6.4f,
				BAminman11 = 6.35f,
				BAminman12 = 5,
				BAminman13 = 4.95f;


			/*Nultijd balvaardigheid bepalen*/
			double Balvaardigheid0 = new double();
			if (GES == 1 && LFT == 6)
			{
				Balvaardigheid0 = BAmaxman6;
			}
			else if (GES == 1 && LFT == 7)
			{
				Balvaardigheid0 = BAmaxman7;
			}

			else if (GES == 1 && LFT == 8)
			{
				Balvaardigheid0 = BAmaxman8;
			}

			else if (GES == 1 && LFT == 9)
			{
				Balvaardigheid0 = BAmaxman9;
			}

			else if (GES == 1 && LFT == 10)
			{
				Balvaardigheid0 = BAmaxman10;
			}

			else if (GES == 1 && LFT == 11)
			{
				Balvaardigheid0 = BAmaxman11;
			}

			else if (GES == 1 && LFT == 12)
			{
				Balvaardigheid0 = BAmaxman12;
			}

			else if (GES == 1 && LFT == 13)
			{
				Balvaardigheid0 = BAmaxman13;
			}

			else if (GES == 2 && LFT == 6)
			{
				Balvaardigheid0 = BAmaxvrouw6;
			}

			else if (GES == 2 && LFT == 7)
			{
				Balvaardigheid0 = BAmaxvrouw7;
			}

			else if (GES == 2 && LFT == 8)
			{
				Balvaardigheid0 = BAmaxvrouw8;
			}

			else if (GES == 2 && LFT == 9)
			{
				Balvaardigheid0 = BAmaxvrouw9;
			}

			else if (GES == 2 && LFT == 10)
			{
				Balvaardigheid0 = BAmaxvrouw10;
			}

			else if (GES == 2 && LFT == 11)
			{
				Balvaardigheid0 = BAmaxvrouw11;
			}

			else if (GES == 2 && LFT == 12)
			{
				Balvaardigheid0 = BAmaxvrouw12;
			}

			else if (GES == 2 && LFT == 13)
			{
				Balvaardigheid0 = BAmaxvrouw13;
			}

			/*Honderdtijd balvaardigheid bepalen*/
			double Balvaardigheid100 = new double();
			if (GES == 1 && LFT == 6)
			{
				Balvaardigheid100 = BAminman6;
			}
			else if (GES == 1 && LFT == 7)
			{
				Balvaardigheid100 = BAminman7;
			}

			else if (GES == 1 && LFT == 8)
			{
				Balvaardigheid100 = BAminman8;
			}

			else if (GES == 1 && LFT == 9)
			{
				Balvaardigheid100 = BAminman9;
			}

			else if (GES == 1 && LFT == 10)
			{
				Balvaardigheid100 = BAminman10;
			}

			else if (GES == 1 && LFT == 11)
			{
				Balvaardigheid100 = BAminman11;
			}

			else if (GES == 1 && LFT == 12)
			{
				Balvaardigheid100 = BAminman12;
			}

			else if (GES == 1 && LFT == 13)
			{
				Balvaardigheid100 = BAminman13;
			}

			else if (GES == 2 && LFT == 6)
			{
				Balvaardigheid100 = BAminvrouw6;
			}

			else if (GES == 2 && LFT == 7)
			{
				Balvaardigheid100 = BAminvrouw7;
			}

			else if (GES == 2 && LFT == 8)
			{
				Balvaardigheid100 = BAminvrouw8;
			}

			else if (GES == 2 && LFT == 9)
			{
				Balvaardigheid100 = BAminvrouw9;
			}

			else if (GES == 2 && LFT == 10)
			{
				Balvaardigheid100 = BAminvrouw10;
			}

			else if (GES == 2 && LFT == 11)
			{
				Balvaardigheid100 = BAminvrouw11;
			}

			else if (GES == 2 && LFT == 12)
			{
				Balvaardigheid100 = BAminvrouw12;
			}

			else if (GES == 2 && LFT == 13)
			{
				Balvaardigheid100 = BAminvrouw13;
			}

			/*Verschil in tijd met snelste*/
			double BAafstandsnelste = Tbalvaardigheid - Balvaardigheid100;
			double BApositie = new double();
			/*Percentage kind bepalen*/
			if (Tbalvaardigheid < Balvaardigheid100)
			{
				BApositie = 100;
			}

			else if (Tbalvaardigheid > Balvaardigheid0)
			{
				BApositie = 0;
			}

			else
			{
				BApositie = 100 - ((BAafstandsnelste / (Balvaardigheid0 - Balvaardigheid100)) * 100);
			}

			/*Blauwe grens balvaardigheid bepalen*/
			double BAblauw = new double();
			if (GES == 1 && LFT == 6)
			{
				BAblauw = BAblauwman6;
			}
			else if (GES == 1 && LFT == 7)
			{
				BAblauw = BAblauwman7;
			}

			else if (GES == 1 && LFT == 8)
			{
				BAblauw = BAblauwman8;
			}

			else if (GES == 1 && LFT == 9)
			{
				BAblauw = BAblauwman9;
			}

			else if (GES == 1 && LFT == 10)
			{
				BAblauw = BAblauwman10;
			}

			else if (GES == 1 && LFT == 11)
			{
				BAblauw = BAblauwman11;
			}

			else if (GES == 1 && LFT == 12)
			{
				BAblauw = BAblauwman12;
			}

			else if (GES == 1 && LFT == 13)
			{
				BAblauw = BAblauwman13;
			}

			else if (GES == 2 && LFT == 6)
			{
				BAblauw = BAblauwvrouw6;
			}

			else if (GES == 2 && LFT == 7)
			{
				BAblauw = BAblauwvrouw7;
			}

			else if (GES == 2 && LFT == 8)
			{
				BAblauw = BAblauwvrouw8;
			}

			else if (GES == 2 && LFT == 9)
			{
				BAblauw = BAblauwvrouw9;
			}

			else if (GES == 2 && LFT == 10)
			{
				BAblauw = BAblauwvrouw10;
			}

			else if (GES == 2 && LFT == 11)
			{
				BAblauw = BAblauwvrouw11;
			}

			else if (GES == 2 && LFT == 12)
			{
				BAblauw = BAblauwvrouw12;
			}

			else if (GES == 2 && LFT == 13)
			{
				BAblauw = BAblauwvrouw13;
			}

			/*Oranje grens balvaardigheid bepalen*/
			double BAoranje;
			if (GES == 1 && LFT == 6)
			{
				BAoranje = BAoranjeman6;
			}
			else if (GES == 1 && LFT == 7)
			{
				BAoranje = BAoranjeman7;
			}

			else if (GES == 1 && LFT == 8)
			{
				BAoranje = BAoranjeman8;
			}

			else if (GES == 1 && LFT == 9)
			{
				BAoranje = BAoranjeman9;
			}

			else if (GES == 1 && LFT == 10)
			{
				BAoranje = BAoranjeman10;
			}

			else if (GES == 1 && LFT == 11)
			{
				BAoranje = BAoranjeman11;
			}

			else if (GES == 1 && LFT == 12)
			{
				BAoranje = BAoranjeman12;
			}

			else if (GES == 1 && LFT == 13)
			{
				BAoranje = BAoranjeman13;
			}

			else if (GES == 2 && LFT == 6)
			{
				BAoranje = BAoranjevrouw6;
			}

			else if (GES == 2 && LFT == 7)
			{
				BAoranje = BAoranjevrouw7;
			}

			else if (GES == 2 && LFT == 8)
			{
				BAoranje = BAoranjevrouw8;
			}

			else if (GES == 2 && LFT == 9)
			{
				BAoranje = BAoranjevrouw9;
			}

			else if (GES == 2 && LFT == 10)
			{
				BAoranje = BAoranjevrouw10;
			}

			else if (GES == 2 && LFT == 11)
			{
				BAoranje = BAoranjevrouw11;
			}

			else if (GES == 2 && LFT == 12)
			{
				BAoranje = BAoranjevrouw12;
			}

			else if (GES == 2 && LFT == 13)
			{
				BAoranje = BAoranjevrouw13;
			}

			/*--> Rollen <--*/

			/*Blauw tijd voor rollen bij vrouwen*/
			double Rblauwvrouw6 = 6.8f,
				Rblauwvrouw7 = 6.8f,
				Rblauwvrouw8 = 5.9f,
				Rblauwvrouw9 = 5.9f,
				Rblauwvrouw10 = 5.3f,
				Rblauwvrouw11 = 5.3f,
				Rblauwvrouw12 = 4.8f,
				Rblauwvrouw13 = 4.8f,

				/*Oranje tijd voor rollen bij vrouwen*/
				Roranjevrouw6 = 13.2f,
				Roranjevrouw7 = 13.2f,
				Roranjevrouw8 = 12.8f,
				Roranjevrouw9 = 12.1f,
				Roranjevrouw10 = 11.8f,
				Roranjevrouw11 = 11.9f,
				Roranjevrouw12 = 12.6f,
				Roranjevrouw13 = 12.6f,

				/*Maximale tijd voor rollen bij vrouwen*/
				Rmaxvrouw6 = 16.5f,
				Rmaxvrouw7 = 16.4f,
				Rmaxvrouw8 = 16,
				Rmaxvrouw9 = 15.6f,
				Rmaxvrouw10 = 15.1f,
				Rmaxvrouw11 = 15,
				Rmaxvrouw12 = 16,
				Rmaxvrouw13 = 16,

				/*Minimale tijd voor rollen bij vrouwen*/
				Rminvrouw6 = 4.2f,
				Rminvrouw7 = 4.1f,
				Rminvrouw8 = 3.2f,
				Rminvrouw9 = 3.1f,
				Rminvrouw10 = 2.7f,
				Rminvrouw11 = 2.7f,
				Rminvrouw12 = 2.25f,
				Rminvrouw13 = 2.2f,

				/*Blauw tijd voor rollen bij mannen*/
				Rblauwman6 = 7.86f,
				Rblauwman7 = 7.86f,
				Rblauwman8 = 6.75f,
				Rblauwman9 = 5.89f,
				Rblauwman10 = 4.52f,
				Rblauwman11 = 4.53f,
				Rblauwman12 = 4.12f,
				Rblauwman13 = 4.12f,

				/*Oranje tijd voor rollen bij mannen*/
				Roranjeman6 = 17.3f,
				Roranjeman7 = 17.3f,
				Roranjeman8 = 14,
				Roranjeman9 = 14,
				Roranjeman10 = 12,
				Roranjeman11 = 11.5f,
				Roranjeman12 = 10.9f,
				Roranjeman13 = 10.9f,

				/*Maximale tijd voor rollen bij mannen*/
				Rmaxman6 = 21,
				Rmaxman7 = 20.95f,
				Rmaxman8 = 17.17f,
				Rmaxman9 = 17.17f,
				Rmaxman10 = 14.7f,
				Rmaxman11 = 14.2f,
				Rmaxman12 = 13.5f,
				Rmaxman13 = 13.45f,

				/*Minimale tijd voor rollen bij mannen*/
				Rminman6 = 4.5f,
				Rminman7 = 4.45f,
				Rminman8 = 3.8f,
				Rminman9 = 2.6f,
				Rminman10 = 2,
				Rminman11 = 2,
				Rminman12 = 1.6f,
				Rminman13 = 1.55f;


			/*Nultijd rollen bepalen*/
			double Rollen0 = new double();
			if (GES == 1 && LFT == 6)
			{
				Rollen0 = Rmaxman6;
			}
			else if (GES == 1 && LFT == 7)
			{
				Rollen0 = Rmaxman7;
			}

			else if (GES == 1 && LFT == 8)
			{
				Rollen0 = Rmaxman8;
			}

			else if (GES == 1 && LFT == 9)
			{
				Rollen0 = Rmaxman9;
			}

			else if (GES == 1 && LFT == 10)
			{
				Rollen0 = Rmaxman10;
			}

			else if (GES == 1 && LFT == 11)
			{
				Rollen0 = Rmaxman11;
			}

			else if (GES == 1 && LFT == 12)
			{
				Rollen0 = Rmaxman12;
			}

			else if (GES == 1 && LFT == 13)
			{
				Rollen0 = Rmaxman13;
			}

			else if (GES == 2 && LFT == 6)
			{
				Rollen0 = Rmaxvrouw6;
			}

			else if (GES == 2 && LFT == 7)
			{
				Rollen0 = Rmaxvrouw7;
			}

			else if (GES == 2 && LFT == 8)
			{
				Rollen0 = Rmaxvrouw8;
			}

			else if (GES == 2 && LFT == 9)
			{
				Rollen0 = Rmaxvrouw9;
			}

			else if (GES == 2 && LFT == 10)
			{
				Rollen0 = Rmaxvrouw10;
			}

			else if (GES == 2 && LFT == 11)
			{
				Rollen0 = Rmaxvrouw11;
			}

			else if (GES == 2 && LFT == 12)
			{
				Rollen0 = Rmaxvrouw12;
			}

			else if (GES == 2 && LFT == 13)
			{
				Rollen0 = Rmaxvrouw13;
			}

			/*Honderdtijd rollen bepalen*/
			double Rollen100 = new double();
			if (GES == 1 && LFT == 6)
			{
				Rollen100 = Rminman6;
			}
			else if (GES == 1 && LFT == 7)
			{
				Rollen100 = Rminman7;
			}

			else if (GES == 1 && LFT == 8)
			{
				Rollen100 = Rminman8;
			}

			else if (GES == 1 && LFT == 9)
			{
				Rollen100 = Rminman9;
			}

			else if (GES == 1 && LFT == 10)
			{
				Rollen100 = Rminman10;
			}

			else if (GES == 1 && LFT == 11)
			{
				Rollen100 = Rminman11;
			}

			else if (GES == 1 && LFT == 12)
			{
				Rollen100 = Rminman12;
			}

			else if (GES == 1 && LFT == 13)
			{
				Rollen100 = Rminman13;
			}

			else if (GES == 2 && LFT == 6)
			{
				Rollen100 = Rminvrouw6;
			}

			else if (GES == 2 && LFT == 7)
			{
				Rollen100 = Rminvrouw7;
			}

			else if (GES == 2 && LFT == 8)
			{
				Rollen100 = Rminvrouw8;
			}

			else if (GES == 2 && LFT == 9)
			{
				Rollen100 = Rminvrouw9;
			}

			else if (GES == 2 && LFT == 10)
			{
				Rollen100 = Rminvrouw10;
			}

			else if (GES == 2 && LFT == 11)
			{
				Rollen100 = Rminvrouw11;
			}

			else if (GES == 2 && LFT == 12)
			{
				Rollen100 = Rminvrouw12;
			}

			else if (GES == 2 && LFT == 13)
			{
				Rollen100 = Rminvrouw13;
			}

			/*Verschil in tijd met snelste*/
			double Rafstandsnelste = Trollen - Rollen100;
			double Rpositie = new double();
			/*Percentage kind bepalen*/
			if (Trollen < Rollen100)
			{
				Rpositie = 100;
			}

			else if (Trollen > Rollen0)
			{
				Rpositie = 0;
			}

			else
			{
				Rpositie = 100 - ((Rafstandsnelste / (Rollen0 - Rollen100)) * 100);
			}

			/*Blauwe grens rollen bepalen*/
			double Rblauw;
			if (GES == 1 && LFT == 6)
			{
				Rblauw = Rblauwman6;
			}
			else if (GES == 1 && LFT == 7)
			{
				Rblauw = Rblauwman7;
			}

			else if (GES == 1 && LFT == 8)
			{
				Rblauw = Rblauwman8;
			}

			else if (GES == 1 && LFT == 9)
			{
				Rblauw = Rblauwman9;
			}

			else if (GES == 1 && LFT == 10)
			{
				Rblauw = Rblauwman10;
			}

			else if (GES == 1 && LFT == 11)
			{
				Rblauw = Rblauwman11;
			}

			else if (GES == 1 && LFT == 12)
			{
				Rblauw = Rblauwman12;
			}

			else if (GES == 1 && LFT == 13)
			{
				Rblauw = Rblauwman13;
			}

			else if (GES == 2 && LFT == 6)
			{
				Rblauw = Rblauwvrouw6;
			}

			else if (GES == 2 && LFT == 7)
			{
				Rblauw = Rblauwvrouw7;
			}

			else if (GES == 2 && LFT == 8)
			{
				Rblauw = Rblauwvrouw8;
			}

			else if (GES == 2 && LFT == 9)
			{
				Rblauw = Rblauwvrouw9;
			}

			else if (GES == 2 && LFT == 10)
			{
				Rblauw = Rblauwvrouw10;
			}

			else if (GES == 2 && LFT == 11)
			{
				Rblauw = Rblauwvrouw11;
			}

			else if (GES == 2 && LFT == 12)
			{
				Rblauw = Rblauwvrouw12;
			}

			else if (GES == 2 && LFT == 13)
			{
				Rblauw = Rblauwvrouw13;
			}

			/*Oranje grens rollen bepalen*/
			double Roranje;
			if (GES == 1 && LFT == 6)
			{
				Roranje = Roranjeman6;
			}
			else if (GES == 1 && LFT == 7)
			{
				Roranje = Roranjeman7;
			}

			else if (GES == 1 && LFT == 8)
			{
				Roranje = Roranjeman8;
			}

			else if (GES == 1 && LFT == 9)
			{
				Roranje = Roranjeman9;
			}

			else if (GES == 1 && LFT == 10)
			{
				Roranje = Roranjeman10;
			}

			else if (GES == 1 && LFT == 11)
			{
				Roranje = Roranjeman11;
			}

			else if (GES == 1 && LFT == 12)
			{
				Roranje = Roranjeman12;
			}

			else if (GES == 1 && LFT == 13)
			{
				Roranje = Roranjeman13;
			}

			else if (GES == 2 && LFT == 6)
			{
				Roranje = Roranjevrouw6;
			}

			else if (GES == 2 && LFT == 7)
			{
				Roranje = Roranjevrouw7;
			}

			else if (GES == 2 && LFT == 8)
			{
				Roranje = Roranjevrouw8;
			}

			else if (GES == 2 && LFT == 9)
			{
				Roranje = Roranjevrouw9;
			}

			else if (GES == 2 && LFT == 10)
			{
				Roranje = Roranjevrouw10;
			}

			else if (GES == 2 && LFT == 11)
			{
				Roranje = Roranjevrouw11;
			}

			else if (GES == 2 && LFT == 12)
			{
				Roranje = Roranjevrouw12;
			}

			else if (GES == 2 && LFT == 13)
			{
				Roranje = Roranjevrouw13;
			}


			/*--> Behendigheid <--*/

			/*Blauw tijd voor behendigheid bij vrouwen*/
			double BEblauwvrouw6 = 10.4f,
				BEblauwvrouw7 = 10.4f,
				BEblauwvrouw8 = 8,
				BEblauwvrouw9 = 8,
				BEblauwvrouw10 = 6.6f,
				BEblauwvrouw11 = 6.5f,
				BEblauwvrouw12 = 5.7f,
				BEblauwvrouw13 = 5.7f,

				/*Oranje tijd voor behendigheid bij vrouwen*/
				BEoranjevrouw6 = 17.3f,
				BEoranjevrouw7 = 17.3f,
				BEoranjevrouw8 = 14,
				BEoranjevrouw9 = 14,
				BEoranjevrouw10 = 12,
				BEoranjevrouw11 = 11.5f,
				BEoranjevrouw12 = 10.9f,
				BEoranjevrouw13 = 10.9f,

				/*Maximale tijd voor behendigheid bij vrouwen*/
				BEmaxvrouw6 = 21,
				BEmaxvrouw7 = 20.95f,
				BEmaxvrouw8 = 17,
				BEmaxvrouw9 = 16.95f,
				BEmaxvrouw10 = 15,
				BEmaxvrouw11 = 14,
				BEmaxvrouw12 = 13,
				BEmaxvrouw13 = 12.95f,

				/*Minimale tijd voor behendigheid bij vrouwen*/
				BEminvrouw6 = 7,
				BEminvrouw7 = 7.15f,
				BEminvrouw8 = 5,
				BEminvrouw9 = 4.95f,
				BEminvrouw10 = 4.2f,
				BEminvrouw11 = 4.15f,
				BEminvrouw12 = 3.8f,
				BEminvrouw13 = 3.75f,

				/*Blauw tijd voor behendigheid bij mannen*/
				BEblauwman6 = 10.2f,
				BEblauwman7 = 10.2f,
				BEblauwman8 = 7.8f,
				BEblauwman9 = 7.3f,
				BEblauwman10 = 5.8f,
				BEblauwman11 = 5.8f,
				BEblauwman12 = 5,
				BEblauwman13 = 5,

				/*Oranje tijd voor behendigheid bij mannen*/
				BEoranjeman6 = 17.6f,
				BEoranjeman7 = 17.6f,
				BEoranjeman8 = 13.7f,
				BEoranjeman9 = 13.5f,
				BEoranjeman10 = 10.5f,
				BEoranjeman11 = 10.6f,
				BEoranjeman12 = 10.2f,
				BEoranjeman13 = 10.2f,

				/*Maximale tijd voor behendigheid bij mannen*/
				BEmaxman6 = 21.5f,
				BEmaxman7 = 21.45f,
				BEmaxman8 = 16.65f,
				BEmaxman9 = 16.5f,
				BEmaxman10 = 12.8f,
				BEmaxman11 = 12.95f,
				BEmaxman12 = 12.7f,
				BEmaxman13 = 12.65f,

				/*Minimale tijd voor behendigheid bij mannen*/
				BEminman6 = 7,
				BEminman7 = 6.95f,
				BEminman8 = 4.9f,
				BEminman9 = 4.5f,
				BEminman10 = 3.6f,
				BEminman11 = 3.55f,
				BEminman12 = 3,
				BEminman13 = 2.95f;

			/*Nultijd behendigheid bepalen*/
			double Behendigheid0 = new double();
			if (GES == 1 && LFT == 6)
			{
				Behendigheid0 = BEmaxman6;
			}
			else if (GES == 1 && LFT == 7)
			{
				Behendigheid0 = BEmaxman7;
			}

			else if (GES == 1 && LFT == 8)
			{
				Behendigheid0 = BEmaxman8;
			}

			else if (GES == 1 && LFT == 9)
			{
				Behendigheid0 = BEmaxman9;
			}

			else if (GES == 1 && LFT == 10)
			{
				Behendigheid0 = BEmaxman10;
			}

			else if (GES == 1 && LFT == 11)
			{
				Behendigheid0 = BEmaxman11;
			}

			else if (GES == 1 && LFT == 12)
			{
				Behendigheid0 = BEmaxman12;
			}

			else if (GES == 1 && LFT == 13)
			{
				Behendigheid0 = BEmaxman13;
			}

			else if (GES == 2 && LFT == 6)
			{
				Behendigheid0 = BEmaxvrouw6;
			}

			else if (GES == 2 && LFT == 7)
			{
				Behendigheid0 = BEmaxvrouw7;
			}

			else if (GES == 2 && LFT == 8)
			{
				Behendigheid0 = BEmaxvrouw8;
			}

			else if (GES == 2 && LFT == 9)
			{
				Behendigheid0 = BEmaxvrouw9;
			}

			else if (GES == 2 && LFT == 10)
			{
				Behendigheid0 = BEmaxvrouw10;
			}

			else if (GES == 2 && LFT == 11)
			{
				Behendigheid0 = BEmaxvrouw11;
			}

			else if (GES == 2 && LFT == 12)
			{
				Behendigheid0 = BEmaxvrouw12;
			}

			else if (GES == 2 && LFT == 13)
			{
				Behendigheid0 = BEmaxvrouw13;
			}

			/*Honderdtijd behendigheid bepalen*/
			double Behendigheid100 = new double();
			if (GES == 1 && LFT == 6)
			{
				Behendigheid100 = BEminman6;
			}
			else if (GES == 1 && LFT == 7)
			{
				Behendigheid100 = BEminman7;
			}

			else if (GES == 1 && LFT == 8)
			{
				Behendigheid100 = BEminman8;
			}

			else if (GES == 1 && LFT == 9)
			{
				Behendigheid100 = BEminman9;
			}

			else if (GES == 1 && LFT == 10)
			{
				Behendigheid100 = BEminman10;
			}

			else if (GES == 1 && LFT == 11)
			{
				Behendigheid100 = BEminman11;
			}

			else if (GES == 1 && LFT == 12)
			{
				Behendigheid100 = BEminman12;
			}

			else if (GES == 1 && LFT == 13)
			{
				Behendigheid100 = BEminman13;
			}

			else if (GES == 2 && LFT == 6)
			{
				Behendigheid100 = BEminvrouw6;
			}

			else if (GES == 2 && LFT == 7)
			{
				Behendigheid100 = BEminvrouw7;
			}

			else if (GES == 2 && LFT == 8)
			{
				Behendigheid100 = BEminvrouw8;
			}

			else if (GES == 2 && LFT == 9)
			{
				Behendigheid100 = BEminvrouw9;
			}

			else if (GES == 2 && LFT == 10)
			{
				Behendigheid100 = BEminvrouw10;
			}

			else if (GES == 2 && LFT == 11)
			{
				Behendigheid100 = BEminvrouw11;
			}

			else if (GES == 2 && LFT == 12)
			{
				Behendigheid100 = BEminvrouw12;
			}

			else if (GES == 2 && LFT == 13)
			{
				Behendigheid100 = BEminvrouw13;
			}

			/*Verschil in tijd met snelste*/
			double BEafstandsnelste = Tbehendigheid - Behendigheid100;
			double BEpositie = new double();
			/*Percentage kind bepalen*/
			if (Tbehendigheid < Behendigheid100)
			{
				BEpositie = 100;
			}

			else if (Tbehendigheid > Behendigheid0)
			{
				BEpositie = 0;
			}

			else
			{
				BEpositie = 100 - ((BEafstandsnelste / (Behendigheid0 - Behendigheid100)) * 100);
			}

			/*Blauwe grens behendigheid bepalen*/
			double BEblauw = new double();
			if (GES == 1 && LFT == 6)
			{
				BEblauw = BEblauwman6;
			}
			else if (GES == 1 && LFT == 7)
			{
				BEblauw = BEblauwman7;
			}

			else if (GES == 1 && LFT == 8)
			{
				BEblauw = BEblauwman8;
			}

			else if (GES == 1 && LFT == 9)
			{
				BEblauw = BEblauwman9;
			}

			else if (GES == 1 && LFT == 10)
			{
				BEblauw = BEblauwman10;
			}

			else if (GES == 1 && LFT == 11)
			{
				BEblauw = BEblauwman11;
			}

			else if (GES == 1 && LFT == 12)
			{
				BEblauw = BEblauwman12;
			}

			else if (GES == 1 && LFT == 13)
			{
				BEblauw = BEblauwman13;
			}

			else if (GES == 2 && LFT == 6)
			{
				BEblauw = BEblauwvrouw6;
			}

			else if (GES == 2 && LFT == 7)
			{
				BEblauw = BEblauwvrouw7;
			}

			else if (GES == 2 && LFT == 8)
			{
				BEblauw = BEblauwvrouw8;
			}

			else if (GES == 2 && LFT == 9)
			{
				BEblauw = BEblauwvrouw9;
			}

			else if (GES == 2 && LFT == 10)
			{
				BEblauw = BEblauwvrouw10;
			}

			else if (GES == 2 && LFT == 11)
			{
				BEblauw = BEblauwvrouw11;
			}

			else if (GES == 2 && LFT == 12)
			{
				BEblauw = BEblauwvrouw12;
			}

			else if (GES == 2 && LFT == 13)
			{
				BEblauw = BEblauwvrouw13;
			}

			/*Oranje grens behendigheid bepalen*/
			double BEoranje = new double();
			if (GES == 1 && LFT == 6)
			{
				BEoranje = BEoranjeman6;
			}
			else if (GES == 1 && LFT == 7)
			{
				BEoranje = BEoranjeman7;
			}

			else if (GES == 1 && LFT == 8)
			{
				BEoranje = BEoranjeman8;
			}

			else if (GES == 1 && LFT == 9)
			{
				BEoranje = BEoranjeman9;
			}

			else if (GES == 1 && LFT == 10)
			{
				BEoranje = BEoranjeman10;
			}

			else if (GES == 1 && LFT == 11)
			{
				BEoranje = BEoranjeman11;
			}

			else if (GES == 1 && LFT == 12)
			{
				BEoranje = BEoranjeman12;
			}

			else if (GES == 1 && LFT == 13)
			{
				BEoranje = BEoranjeman13;
			}

			else if (GES == 2 && LFT == 6)
			{
				BEoranje = BEoranjevrouw6;
			}

			else if (GES == 2 && LFT == 7)
			{
				BEoranje = BEoranjevrouw7;
			}

			else if (GES == 2 && LFT == 8)
			{
				BEoranje = BEoranjevrouw8;
			}

			else if (GES == 2 && LFT == 9)
			{
				BEoranje = BEoranjevrouw9;
			}

			else if (GES == 2 && LFT == 10)
			{
				BEoranje = BEoranjevrouw10;
			}

			else if (GES == 2 && LFT == 11)
			{
				BEoranje = BEoranjevrouw11;
			}

			else if (GES == 2 && LFT == 12)
			{
				BEoranje = BEoranjevrouw12;
			}

			else if (GES == 2 && LFT == 13)
			{
				BEoranje = BEoranjevrouw13;
			}




			/*--> Berekening voor grafieken <--*/

			/* Tegenposities bepalen */
			double Toverig = 100 - Tpositie,
				Soverig = 100 - Spositie,
				BAoverig = 100 - BApositie,
				Roverig = 100 - Rpositie,
				BEoverig = 100 - BEpositie;

			double Tpunten = Tpositie / 10,
				Spunten = Spositie / 10,
				BApunten = BApositie / 10,
				Rpunten = Rpositie / 10,
				BEpunten = BEpositie / 10;

			double[] scores = { Tpositie, Spositie, BApositie, Rpositie, BEpositie };

			return scores;
		}
    }
}
