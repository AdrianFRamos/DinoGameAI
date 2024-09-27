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
    internal class DNAs
    {
        double[][] DNADaVez = new double[POPULACAO_TAMANHO][];

        double[] MediaFitnessPopulacao = new double[LARG_GRAFICO];
        double[] MediaFitnessFilhos = new double[LARG_GRAFICO];
        double[] BestFitnessPopulacao = new double[LARG_GRAFICO];

        int GeracaoCompleta = 0;

        double BestFitnessGeracao()
        {
            double maior = 0;
            for (int i = 0; i < POPULACAO_TAMANHO; i++)
            {
                if (Dinossauros[i].Fitness > maior)
                {
                    maior = Dinossauros[i].Fitness;
                }
            }
            return maior;
        }

        double MediaFitnessGeracao()
        {
            double media = 0;
            for (int i = 0; i < POPULACAO_TAMANHO; i++)
            {
                media += Dinossauros[i].Fitness;
            }
            media /= POPULACAO_TAMANHO;
            return media;
        }

        double BestFitnessEver()
        {
            double maior = 0;
            for (int i = 0; i < GeracaoCompleta; i++)
            {
                if (BestFitnessPopulacao[i] > maior)
                {
                    maior = BestFitnessPopulacao[i];
                }
            }
            return maior;
        }

        void InicializarDNA()
        {
            int tamanhoDNA = Dinossauros[0].TamanhoDNA;

            for (int i = 0; i < POPULACAO_TAMANHO; i++)
            {
                DNADaVez[i] = new double[tamanhoDNA];
                for (int j = 0; j < tamanhoDNA; j++)
                {
                    DNADaVez[i][j] = GetRandomValue(); // Substitua getRandomValue() pela sua implementação de aleatoriedade.
                }
            }
        }

    }
}
