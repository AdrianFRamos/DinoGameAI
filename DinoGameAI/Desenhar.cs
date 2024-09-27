using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using static DinoGameAI.Tipos;
using static DinoGameAI.Game;
using static DinoGameAI.Variaveis;

namespace DinoGameAI
{
    internal class Desenhar
    {
        public PIG_Cor CalcularCor(double intensidade, PIG_Cor corBase)
        {
            corBase.r *= intensidade;
            corBase.g *= intensidade;
            corBase.b *= intensidade;

            return corBase;
        }

        void DesenharRedeNeural(int x, int y, int largura, int altura)
        {
            double[] neuroEntradaX = new double[DINO_BRAIN_QTD_INPUT];
            double[] neuroEntradaY = new double[DINO_BRAIN_QTD_INPUT];
            double[,] neuroEscondidoX = new double[DINO_BRAIN_QTD_LAYERS, DINO_BRAIN_QTD_HIDE];
            double[,] neuroEscondidoY = new double[DINO_BRAIN_QTD_LAYERS, DINO_BRAIN_QTD_HIDE];
            double[] neuroSaidaX = new double[DINO_BRAIN_QTD_OUTPUT];
            double[] neuroSaidaY = new double[DINO_BRAIN_QTD_OUTPUT];

            double[] entrada = new double[DINO_BRAIN_QTD_INPUT];
            double xOrigem = x + 325;
            double yOrigem = y + altura;
            double larguraPintura = largura;
            double alturaPintura = altura;
            double tamanhoNeuronio = 20;
            string str = new string(' ', 1000); // Para simular um array de char
            int sprite;
            int qtdEscondidas = MelhorDinossauro.Cerebro.QuantidadeEscondidas;
            int qtdNeuroEntrada = MelhorDinossauro.Cerebro.CamadaEntrada.QuantidadeNeuronios;
            int qtdNeuroEscondidas = MelhorDinossauro.Cerebro.CamadaEscondida[0].QuantidadeNeuronios;
            int qtdNeuroSaida = MelhorDinossauro.Cerebro.CamadaSaida.QuantidadeNeuronios;

            for (int i = 0; i < DINO_BRAIN_QTD_INPUT; i++)
            {
                entrada[i] = MelhorDinossauro.Cerebro.CamadaEntrada.Neuronios[i].Saida;
            }

            double escalaAltura = ((double)alturaPintura) / (qtdNeuroEscondidas - 1);
            double escalaLargura = ((double)larguraPintura - 475) / (qtdEscondidas + 1);

            string.Format(str, "[Obstáculo] Distancia: {0:F2}", entrada[0]);
            EscreverEsquerda(str, x + 15, yOrigem - 0 * escalaAltura - 5, Fonte);

            string.Format(str, "[Obstáculo] Largura: {0:F2}", entrada[1]);
            EscreverEsquerda(str, x + 15, yOrigem - 1 * escalaAltura - 5, Fonte);

            string.Format(str, "[Obstáculo] Altura: {0:F2}", entrada[2]);
            EscreverEsquerda(str, x + 15, yOrigem - 2 * escalaAltura - 5, Fonte);

            string.Format(str, "[Obstáculo] Comprimento: {0:F2}", entrada[3]);
            EscreverEsquerda(str, x + 15, yOrigem - 3 * escalaAltura - 5, Fonte);

            string.Format(str, "[Cenário] Velocidade: {0:F2}", entrada[4]);
            EscreverEsquerda(str, x + 15, yOrigem - 4 * escalaAltura - 5, Fonte);

            string.Format(str, "[Dinossauro] Altura: {0:F2}", entrada[5]);
            EscreverEsquerda(str, x + 15, yOrigem - 5 * escalaAltura - 5, Fonte);

            double temp = yOrigem - (escalaAltura * (qtdNeuroEscondidas - 2)) / 2.0 + (escalaAltura * (qtdNeuroSaida - 1)) / 2.0;

            string.Format(str, "Pular");
            EscreverEsquerda(str, x + largura - 100, temp - 0 * escalaAltura - 5, Fonte);

            string.Format(str, "Abaixar");
            EscreverEsquerda(str, x + largura - 100, temp - 1 * escalaAltura - 5, Fonte);

            if (DINO_BRAIN_QTD_OUTPUT == 3)
            {
                string.Format(str, "Avião");
                EscreverEsquerda(str, x + largura - 100, temp - 2 * escalaAltura - 5, Fonte);
            }

            // Desenhar Conexoes

            for (int i = 0; i < qtdNeuroEntrada - 1; i++)
            {
                neuroEntradaX[i] = xOrigem;
                neuroEntradaY[i] = yOrigem - i * escalaAltura;
            }

            for (int i = 0; i < qtdEscondidas; i++)
            {
                int qtdCamadaAnterior;
                Camada camdaAnterior;
                double[] xAnterior;
                double[] yAnterior;

                if (i == 0)
                {
                    qtdCamadaAnterior = qtdNeuroEntrada;
                    camdaAnterior = MelhorDinossauro.Cerebro.CamadaEntrada;
                    xAnterior = neuroEntradaX;
                    yAnterior = neuroEntradaY;
                }
                else
                {
                    qtdCamadaAnterior = qtdNeuroEscondidas;
                    camdaAnterior = MelhorDinossauro.Cerebro.CamadaEscondida[i - 1];
                    xAnterior = neuroEscondidoX[i - 1];
                    yAnterior = neuroEscondidoY[i - 1];
                }

                for (int j = 0; j < qtdNeuroEscondidas - 1; j++)
                {
                    neuroEscondidoX[i, j] = xOrigem + (i + 1) * escalaLargura;
                    neuroEscondidoY[i, j] = yOrigem - j * escalaAltura;

                    for (int k = 0; k < qtdCamadaAnterior - 1; k++)
                    {
                        double peso = MelhorDinossauro.Cerebro.CamadaEscondida[i].Neuronios[j].Peso[k];
                        double saida = camdaAnterior.Neuronios[k].Saida;
                        if (peso * saida > 0)
                        {
                            DesenharLinhaSimples(xAnterior[k],
                                                 yAnterior[k],
                                                 neuroEscondidoX[i, j],
                                                 neuroEscondidoY[i, j], VERMELHO);
                        }
                        else
                        {
                            DesenharLinhaSimples(xAnterior[k],
                                                 yAnterior[k],
                                                 neuroEscondidoX[i, j],
                                                 neuroEscondidoY[i, j], CINZA_CLARO);
                        }
                    }
                }
            }

            for (int i = 0; i < qtdNeuroSaida; i++)
            {
                int ultimaCamada = MelhorDinossauro.Cerebro.QuantidadeEscondidas - 1;
                double temp = yOrigem - (escalaAltura * (qtdNeuroEscondidas - 2)) / 2.0 + (escalaAltura * (qtdNeuroSaida - 1)) / 2.0;

                neuroSaidaX[i] = xOrigem + (qtdEscondidas + 1) * escalaLargura;
                neuroSaidaY[i] = temp - i * escalaAltura;

                for (int k = 0; k < qtdNeuroEscondidas - 1; k++)
                {
                    double peso = MelhorDinossauro.Cerebro.CamadaSaida.Neuronios[i].Peso[k];
                    double saida = MelhorDinossauro.Cerebro.CamadaEscondida[ultimaCamada].Neuronios[k].Saida;

                    if (peso * saida > 0)
                    {
                        DesenharLinhaSimples(neuroEscondidoX[ultimaCamada, k],
                                             neuroEscondidoY[ultimaCamada, k],
                                             neuroSaidaX[i],
                                             neuroSaidaY[i], VERMELHO);
                    }
                    else
                    {
                        DesenharLinhaSimples(neuroEscondidoX[ultimaCamada, k],
                                             neuroEscondidoY[ultimaCamada, k],
                                             neuroSaidaX[i],
                                             neuroSaidaY[i], CINZA_CLARO);
                    }
                }
            }

            // Desenhar Neuronios

            for (int i = 0; i < qtdNeuroEntrada - 1; i++)
            {
                PIG_Cor cor;
                double opacidade;

                switch (i)
                {
                    case 0:
                        opacidade = Math.Abs(entrada[0]) / 800.0;
                        cor = calcularCor(opacidade, BRANCO);
                        break;
                    case 1:
                        opacidade = entrada[1] / 73.0;
                        cor = calcularCor(opacidade, BRANCO);
                        break;
                    case 2:
                        opacidade = entrada[2] / 90.0;
                        cor = calcularCor(opacidade, BRANCO);
                        break;
                    case 3:
                        opacidade = entrada[3] / 47.0;
                        cor = calcularCor(opacidade, BRANCO);
                        break;
                    case 4:
                        opacidade = entrada[4] / 8.0;
                        if (entrada[4] > 8)
                        {
                            opacidade = 1;
                        }
                        cor = calcularCor(opacidade, BRANCO);
                        break;
                    case 5:
                        opacidade = entrada[5] / 10.0;
                        cor = calcularCor(opacidade, BRANCO);
                        break;
                    default:
                        opacidade = 0;
                        cor = BRANCO;
                        break;
                }

                DesenharCirculo(neuroEntradaX[i], neuroEntradaY[i], tamanhoNeuronio, cor);
            }

            for (int i = 0; i < qtdEscondidas; i++)
            {
                for (int j = 0; j < qtdNeuroEscondidas - 1; j++)
                {
                    double opacidade = MelhorDinossauro.Cerebro.CamadaEscondida[i].Neuronios[j].Saida;
                    DesenharCirculo(neuroEscondidoX[i, j], neuroEscondidoY[i, j], tamanhoNeuronio, calcularCor(opacidade, BRANCO));
                }
            }

            for (int i = 0; i < qtdNeuroSaida; i++)
            {
                double opacidade = MelhorDinossauro.Cerebro.CamadaSaida.Neuronios[i].Saida;
                DesenharCirculo(neuroSaidaX[i], neuroSaidaY[i], tamanhoNeuronio, calcularCor(opacidade, BRANCO));
            }
        }

        void DesenharGrafico(int x, int y, int largura, int altura)
        {
            SDL_Point[] pontosBest = new SDL_Point[LARG_GRAFICO];
            SDL_Point[] pontosMedia = new SDL_Point[LARG_GRAFICO];
            SDL_Point[] pontosMediaFilhos = new SDL_Point[LARG_GRAFICO];

            double scala;
            double scalaHorizontal;

            if (BestFitnessGeracao() > BestFitnessEver())
            {
                scala = BestFitnessGeracao() / (double)altura;
            }
            else
            {
                scala = BestFitnessEver() / (double)altura;
            }

            if (GeracaoCompleta == 0)
            {
                scalaHorizontal = 0;
            }
            else
            {
                scalaHorizontal = (double)largura / (double)(GeracaoCompleta);
            }

            for (int i = 1; i < 11; i++)
            {
                DesenharLinhaSimples(x, y + i * (altura / 10), x + largura, y + i * (altura / 10), CINZA_CLARO);
                DesenharLinhaSimples(x + i * (largura / 10), y, x + i * (largura / 10), y + altura, CINZA_CLARO);
            }

            DesenharLinhaSimples(x, y, x + largura, y, PRETO);
            DesenharLinhaSimples(x, y, x, y + altura, PRETO);

            for (int i = 0; i < GeracaoCompleta + 1; i++)
            {
                int pontoX = x + 1 + (i * scalaHorizontal);
                double yMedia, yBest;

                if (i == GeracaoCompleta)
                {
                    yMedia = y + 1 + (int)(MediaFitnessGeracao() / scala);
                    yBest = y + 1 + (int)(BestFitnessGeracao() / scala);
                }
                else
                {
                    yMedia = y + 1 + (int)(MediaFitnessPopulacao[i] / scala);
                    yBest = y + 1 + (int)(BestFitnessPopulacao[i] / scala);
                }

                DesenharPonto(pontoX, yMedia, VERMELHO, 3);
                pontosMedia[i] = new SDL_Point { x = pontoX, y = ALT_TELA - (int)yMedia };

                DesenharPonto(pontoX, yBest, AZUL, 3);
                pontosBest[i] = new SDL_Point { x = pontoX, y = ALT_TELA - (int)yBest };
            }

            DesenharLinhas(pontosBest, GeracaoCompleta + 1, AZUL);
            DesenharLinhas(pontosMedia, GeracaoCompleta + 1, VERMELHO);
            // DesenharLinhas(pontosMediaFilhos, GeracaoCompleta, VERDE);
        }

        void DesenharObstaculos()
        {
            for (int i = 0; i < MAX_OBSTACULOS; i++)
            {
                DesenharSprite(obstaculo[i].sprite[obstaculo[i].FrameAtual].Objeto,
                               obstaculo[i].X,
                               obstaculo[i].Y,
                               obstaculo[i].sprite[obstaculo[i].FrameAtual].Largura,
                               obstaculo[i].sprite[obstaculo[i].FrameAtual].Altura,
                               0, 1);
            }
        }

        void DesenharChao()
        {
            for (int i = 0; i < CHAO_QUANTIDADE; i++)
            {
                DesenharSprite(chao[i].sprite.Objeto,
                               chao[i].X,
                               chao[i].Y,
                               chao[i].sprite.Largura,
                               chao[i].sprite.Altura,
                               0);
            }
        }

        void DesenharMontanhas()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    DesenharSprite(montanha[i].sprite[j].Objeto,
                                   montanha[i].X[j], montanha[i].Y[j],
                                   montanha[i].sprite[j].Largura,
                                   montanha[i].sprite[j].Altura, 0);
                }
            }
        }

        void DesenharFundo()
        {
            DesenharRetangulo(0, 0, ALT_TELA, LARG_TELA, BRANCO);
        }

        void DesenharNuvens()
        {
            for (int i = 0; i < NUVEM_QUANTIDADE; i++)
            {
                DesenharSprite(nuvem[i].sprite.Objeto,
                               nuvem[i].X,
                               nuvem[i].Y,
                               nuvem[i].sprite.Largura,
                               nuvem[i].sprite.Altura, 0);
            }
        }

        void DesenharAviao(int i)
        {
            if (Dinossauros[i].Estado == 4)  // Voando
            {
                DesenharSprite(SpriteAviao[Dinossauros[i].FrameAviao].Objeto,
                               Dinossauros[i].X + 13,
                               Dinossauros[i].Y + 12,
                               SpriteAviao[Dinossauros[i].FrameAviao].Largura,
                               SpriteAviao[Dinossauros[i].FrameAviao].Altura, 0, 0);
            }
        }

        void DesenharDinossauros()
        {
            int frame, largura, altura, sprite;

            for (int i = 0; i < QuantidadeDinossauros; i++)
            {
                sprite = Dinossauros[i].SpriteAtual;
                frame = Dinossauros[i].sprite[sprite].Objeto;
                largura = Dinossauros[i].sprite[sprite].Largura;
                altura = Dinossauros[i].sprite[sprite].Altura;

                DesenharSprite(frame,
                               Dinossauros[i].X,
                               Dinossauros[i].Y,
                               largura,
                               altura,
                               0, 1);

                DesenharAviao(i);
            }
        }

        void Desenhar()
        {
            int BASE = 360;

            if (DesenharTela == 1)
            {
                IniciarDesenho();

                DesenharFundo();
                DesenharNuvens();
                DesenharMontanhas();
                DesenharChao();
                DesenharObstaculos();
                DesenharDinossauros();

                DesenharRedeNeural(665, 360, 700, 350);
                DesenharGrafico(20, 390, LARG_GRAFICO, 350);

                string str = $"Geração: {Geracao}";
                EscreverEsquerda(str, 20, BASE, Fonte);

                str = $"Clock: {Periodo:F} segundo";
                EscreverEsquerda(str, 20, BASE - 20, Fonte);

                str = $"Velocidade: {Math.Abs(VELOCIDADE):F2} ({Math.Abs(VELOCIDADE) / Periodo:F0} pixels por segundo)";
                EscreverEsquerda(str, 20, BASE - 40, Fonte);

                str = "Distancia Recorde:";
                EscreverEsquerda(str, 20, BASE - 60, Fonte);

                str = $"{DistanciaRecorde:F0} pixels";
                EscreverEsquerda(str, 150, BASE - 60, FonteAzul);

                str = "Distancia Atual:";
                EscreverEsquerda(str, 20, BASE - 80, Fonte);

                str = $"{DistanciaAtual:F0} pixels";
                EscreverEsquerda(str, 150, BASE - 80, Fonte);

                EncerrarDesenho();
            }
        }

    }
}
