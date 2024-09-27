using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DinoGameAI.Sprites;

namespace DinoGameAI
{
    internal class Tipos
    {
        public const int LARG_GRAFICO = 600;
        public const int CHAO_QUANTIDADE = 30;
        public const int MONTANHA_QUANTIDADE = 3;
        public const int NUVEM_QUANTIDADE = 15;

        public const int QTD_SPRITE_CACTUS = 6;
        public const int MAX_OBSTACULOS = 7;

        public struct Dinossauro
        {
            public double X;
            public double Y;
            public double VelocidadeY;
            public int Frame;
            public int SpriteAtual;
            public int Estado;
            public Sprite[] sprite; // Tamanho fixo de 10 será gerenciado no construtor
            public int TimerFrame;
            public int ResetarFitness;
            public int FrameAviao;
            public int TimerFrameAviao;
            public double AviaoDeslocamento;
            public double AviaoCooldown;
            public int TamanhoDNA;
            public double[] DNA; // Pode ser inicializado como null e gerenciado posteriormente
            public double Fitness;
            public RedeNeural Cerebro;
        }

        public struct Obstaculo
        {
            public double X;
            public double Y;
            public int Tipo;
            public Sprite[] sprite; // Tamanho fixo de 2 será gerenciado no construtor
            public int TimerFrames;
            public int FrameAtual;
        }

        public struct Chao
        {
            public double X;
            public double Y;
            public Sprite sprite;
        }

        public struct Montanha
        {
            public double[] X; // Tamanho fixo de 2 será gerenciado no construtor
            public double[] Y; // Tamanho fixo de 2 será gerenciado no construtor
            public Sprite[] sprite; // Tamanho fixo de 2 será gerenciado no construtor
        }

        public struct Nuvem
        {
            public double X;
            public double Y;
            public Sprite sprite;
        }

        public struct Grafico
        {
            public double[] MediaFitness; // Tamanho fixo de LARG_GRAFICO será gerenciado no construtor
            public double[] MelhorFitness; // Tamanho fixo de LARG_GRAFICO será gerenciado no construtor
        }

    }
}
