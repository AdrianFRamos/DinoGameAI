using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DinoGameAI.Tipos;
using static DinoGameAI.Game;

namespace DinoGameAI
{
    internal class Alocacoes
    {
        public void AlocarDinossauro()
        {
            int ControladorCor = 0;
            int Tamanho;

            for (int i = 0; i < 10; i++)
            {
                Dinossauros[QuantidadeDinossauros].sprite[i] = GetDinossauroSprite(i, Cores[ControladorCor]);
            }

            Dinossauros[QuantidadeDinossauros].TimerFrame = CriarTimer();
            Dinossauros[QuantidadeDinossauros].TimerFrameAviao = CriarTimer();
            Dinossauros[QuantidadeDinossauros].ResetarFitness = 1;

            Dinossauros[QuantidadeDinossauros].Cerebro = RNA_CriarRedeNeural(DINO_BRAIN_QTD_LAYERS,
                                                                             DINO_BRAIN_QTD_INPUT,
                                                                             DINO_BRAIN_QTD_HIDE,
                                                                             DINO_BRAIN_QTD_OUTPUT);

            Tamanho = RNA_QuantidadePesos(Dinossauros[QuantidadeDinossauros].Cerebro);

            Dinossauros[QuantidadeDinossauros].TamanhoDNA = Tamanho;
            Dinossauros[QuantidadeDinossauros].DNA = new double[Tamanho];

            InicializarDinossauro(QuantidadeDinossauros, null, 0, 0);

            ControladorCor = (ControladorCor + 1) % 8;
            QuantidadeDinossauros++;
        }

        public void AlocarDinossauros()
        {
            for (int i = 0; i < POPULACAO_TAMANHO; i++)
            {
                AlocarDinossauro();
            }
        }

        public void AlocarObstaculos()
        {
            for (int i = 0; i < MAX_OBSTACULOS; i++)
            {
                obstaculo[i].TimerFrames = CriarTimer();
            }
        }

    }
}
