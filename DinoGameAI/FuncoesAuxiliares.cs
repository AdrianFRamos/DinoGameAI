using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DinoGameAI
{
    internal class FuncoesAuxiliares
    {
        int ExisteNuvem(double X, double Y)
        {
            for (int i = 0; i < NUVEM_QUANTIDADE; i++)
            {
                if (DistanciaEntrePontos(X, Y, nuvem[i].X, nuvem[i].Y) < 46)
                {
                    return 1;
                }
            }
            return 0;
        }

        double GetRandomValue()
        {
            Random rand = new Random();
            return (rand.Next(20001) / 10.0) - 1000.0;
            //return (rand.Next(201) / 10.0) - 10.0;
            //return (rand.Next(2001) / 1000.0) - 1.0;
            //return (rand.Next(2001) / 10000.0) - 0.1;

            //return rand.Next(3) - 1;
        }

        void GetNextObstaculo(ref Obstaculo obs, int indice)
        {
            obs.X = obstaculosModelo[indice].X;
            obs.Y = obstaculosModelo[indice].Y;
            obs.Tipo = obstaculosModelo[indice].Tipo;

            if (obs.Tipo == PASSARO_CODIGO_TIPO)
            {
                obs.sprite[0] = GetObstaculosSprite(obs.Tipo, 0);
                obs.sprite[1] = GetObstaculosSprite(obs.Tipo, 1);
            }
            else
            {
                obs.sprite[0] = GetObstaculosSprite(obs.Tipo, 0);
                obs.sprite[1] = GetObstaculosSprite(obs.Tipo, 0);
            }
            obs.FrameAtual = 0;
            ReiniciarTimer(obs.TimerFrames);

            ObstaculoDaVez++;
        }

        void SalvarRedeArquivo()
        {
            double maior = 0;
            int indice = 0;
            for (int i = 0; i < POPULACAO_TAMANHO; i++)
            {
                if (Dinossauros[i].Fitness > maior)
                {
                    maior = Dinossauros[i].Fitness;
                    indice = i;
                }
            }

            string filePath = $"redes\\{DistanciaRecorde:F2} - [{DINO_BRAIN_QTD_LAYERS},{DINO_BRAIN_QTD_INPUT},{DINO_BRAIN_QTD_HIDE},{DINO_BRAIN_QTD_OUTPUT}]";

            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            using (BinaryWriter writer = new BinaryWriter(fs))
            {
                writer.Write(Dinossauros[indice].TamanhoDNA);
                for (int j = 0; j < Dinossauros[indice].TamanhoDNA; j++)
                {
                    writer.Write(Dinossauros[indice].DNA[j]);
                }
            }
        }

        void VerificarTeclas()
        {
            if (MODO_JOGO != 1)
            {
                if (PIG_Tecla == TECLA_BAIXO)
                {
                    Periodo /= 2.0;
                }
                if (PIG_Tecla == TECLA_CIMA)
                {
                    Periodo *= 2.0;
                }
            }

            if (PIG_Tecla == TECLA_ESC)
            {
                DesenharTela = -DesenharTela;
            }
        }

        int ProcurarProximoObstaculo(double X)
        {
            int indice = 0;
            double menor = double.MaxValue;
            int largura;

            for (int i = 0; i < MAX_OBSTACULOS; i++)
            {
                largura = obstaculo[i].sprite[obstaculo[i].FrameAtual].Largura;

                if (obstaculo[i].X < menor && obstaculo[i].X + largura > X)
                {
                    menor = obstaculo[i].X;
                    indice = i;
                }
            }

            return indice;
        }

    }
}
