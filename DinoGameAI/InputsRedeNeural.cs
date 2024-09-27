using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DinoGameAI.Tipos;
using static DinoGameAI.Game;
using static DinoGameAI.Variaveis;

namespace DinoGameAI
{
    internal class InputsRedeNeural
    {
        double DistanciaProximoObstaculo(double X)
        {
            int indice = ProcurarProximoObstaculo(X);

            return obstaculo[indice].X - X;
        }

        double LarguraProximoObstaculo(double X)
        {
            int indice = ProcurarProximoObstaculo(X);
            double Largura = obstaculo[indice].sprite[obstaculo[indice].FrameAtual].Largura;

            return Largura;
        }

        double ComprimentoProximoObstaculo(double X)
        {
            int indice = ProcurarProximoObstaculo(X);
            double Altura = obstaculo[indice].sprite[obstaculo[indice].FrameAtual].Altura;

            return Altura;
        }

        double AlturaProximoObstaculo(double X)
        {
            int indice = ProcurarProximoObstaculo(X);

            return obstaculo[indice].Y;
        }

        double AlturaComComprimentoProximoObstaculo(double X)
        {
            int indice = ProcurarProximoObstaculo(X);
            double Comprimento = obstaculo[indice].sprite[obstaculo[indice].FrameAtual].Altura;

            return (obstaculo[indice].Y + Comprimento);
        }

    }
}
