using Domain;

namespace ServiceLayer.Formula
{
    public class FinalScoreFormula
    {
        public int FinalScore(Gender GESLACHT, double LFT, double Ttijgeren, double Tspringen, double Tbalvaardigheid,
			double Trollen, double Tbehendigheid )
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
			int POSITIE = new int();
			if (TTOTAAL < HONDERDTIJD)
			{
				POSITIE = 100;
			}

			else if (TTOTAAL > NULTIJD)
			{
				POSITIE = 0;
			}

			else
			{
				POSITIE = (int)(100 - ((AFSTANDSNELSTE / (NULTIJD - HONDERDTIJD)) * 100));
			}

			return POSITIE;
		}
    }
}