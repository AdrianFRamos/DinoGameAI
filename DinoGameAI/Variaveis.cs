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
        PIG_Cor[] Cores = new PIG_Cor[8]{PIG_Cor.CINZA,PIG_Cor.AMARELO,PIG_Cor.VERDE,PIG_Cor.VERMELHO,PIG_Cor.AZUL,
                PIG_Cor.CIANO,PIG_Cor.LARANJA,PIG_Cor.ROXO
        };

        Dinossauro[] Dinossauros = new Dinossauro[POPULACAO_TAMANHO];
        int QuantidadeDinossauros = 0;

        Chao[] chao = new Chao[CHAO_QUANTIDADE];
        Montanha[] montanha = new Montanha[MONTANHA_QUANTIDADE];
        Nuvem[] nuvem = new Nuvem[NUVEM_QUANTIDADE];
        Grafico grafico;
        Dinossauro MelhorDinossauro;

        Obstaculo[] obstaculo = new Obstaculo[MAX_OBSTACULOS];
        Obstaculo[] obstaculosModelo = new Obstaculo[20000];

        int Fonte, FonteVermelha, FonteAzul;
        double VELOCIDADE;
        int TimerGeral = 0;
        double Periodo = 0.005;
        double DistanciaRecorde, DistanciaAtual;

        int DinossaurosMortos;
        int ObstaculoDaVez = 1;
        int Geracao;
        int DesenharTela = 1;

    }
}
