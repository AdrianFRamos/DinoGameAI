using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DinoGameAI.Tipos;
using static DinoGameAI.Game;

namespace DinoGameAI
{
    public enum PIG_Cor
    {
        CINZA,
        AMARELO,
        VERDE,
        VERMELHO,
        AZUL,
        CIANO,
        LARANJA,
        ROXO
    }

    internal class Variaveis
    {
        // As variáveis agora são públicas e estáticas para permitir o acesso global
        public static PIG_Cor[] Cores = new PIG_Cor[8]{
            PIG_Cor.CINZA, PIG_Cor.AMARELO, PIG_Cor.VERDE, PIG_Cor.VERMELHO,
            PIG_Cor.AZUL, PIG_Cor.CIANO, PIG_Cor.LARANJA, PIG_Cor.ROXO
        };

        public static Dinossauro[] Dinossauros = new Dinossauro[POPULACAO_TAMANHO];
        public static int QuantidadeDinossauros = 0;

        public static Chao[] chao = new Chao[CHAO_QUANTIDADE];
        public static Montanha[] montanha = new Montanha[MONTANHA_QUANTIDADE];
        public static Nuvem[] nuvem = new Nuvem[NUVEM_QUANTIDADE];
        public static Grafico grafico;
        public static Dinossauro MelhorDinossauro;

        public static Obstaculo[] obstaculo = new Obstaculo[MAX_OBSTACULOS];
        public static Obstaculo[] obstaculosModelo = new Obstaculo[20000];

        public static int Fonte;
        public static int FonteVermelha;
        public static int FonteAzul;
        public static double VELOCIDADE;
        public static int TimerGeral = 0;
        public static double Periodo = 0.005;
        public static double DistanciaRecorde;
        public static double DistanciaAtual;

        public static int DinossaurosMortos;
        public static int ObstaculoDaVez = 1;
        public static int Geracao;
        public static int DesenharTela = 1;
    }
}
