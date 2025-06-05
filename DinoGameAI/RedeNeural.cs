using System;
using System.IO;

namespace DinoGameAI
{
    internal class RedeNeural
    {
        private const int BIAS = 1;

        internal class Neuronio
        {
            public double[] Peso = Array.Empty<double>();
            public double Erro;
            public double Saida = 1.0;
            public int QuantidadeLigacoes;
        }

        internal class Camada
        {
            public Neuronio[] Neuronios = Array.Empty<Neuronio>();
            public int QuantidadeNeuronios;
        }

        public Camada CamadaEntrada = new();
        public Camada[] CamadaEscondida = Array.Empty<Camada>();
        public Camada CamadaSaida = new();
        public int QuantidadeEscondidas;

        private static readonly Random random = new();

        private static double Relu(double x) => x < 0 ? 0 : x;
        private static double AtivacaoOcultas(double x) => Relu(x);
        private static double AtivacaoSaida(double x) => Relu(x);

        private static Neuronio CriarNeuronio(int quantidadeLigacoes)
        {
            var neuronio = new Neuronio
            {
                QuantidadeLigacoes = quantidadeLigacoes,
                Peso = new double[quantidadeLigacoes]
            };
            for (int i = 0; i < quantidadeLigacoes; i++)
            {
                neuronio.Peso[i] = random.Next(-1000, 1000);
            }
            return neuronio;
        }

        public static RedeNeural RNA_CriarRedeNeural(int quantidadeEscondidas, int qtdNeuroniosEntrada, int qtdNeuroniosEscondida, int qtdNeuroniosSaida)
        {
            qtdNeuroniosEntrada += BIAS;
            qtdNeuroniosEscondida += BIAS;

            var rede = new RedeNeural
            {
                QuantidadeEscondidas = quantidadeEscondidas,
                CamadaEntrada = new Camada
                {
                    QuantidadeNeuronios = qtdNeuroniosEntrada,
                    Neuronios = new Neuronio[qtdNeuroniosEntrada]
                },
                CamadaEscondida = new Camada[quantidadeEscondidas],
                CamadaSaida = new Camada
                {
                    QuantidadeNeuronios = qtdNeuroniosSaida,
                    Neuronios = new Neuronio[qtdNeuroniosSaida]
                }
            };

            for (int i = 0; i < qtdNeuroniosEntrada; i++)
            {
                rede.CamadaEntrada.Neuronios[i] = new Neuronio();
            }

            for (int i = 0; i < quantidadeEscondidas; i++)
            {
                rede.CamadaEscondida[i] = new Camada
                {
                    QuantidadeNeuronios = qtdNeuroniosEscondida,
                    Neuronios = new Neuronio[qtdNeuroniosEscondida]
                };

                for (int j = 0; j < qtdNeuroniosEscondida; j++)
                {
                    int ligacoes = i == 0 ? qtdNeuroniosEntrada : qtdNeuroniosEscondida;
                    rede.CamadaEscondida[i].Neuronios[j] = CriarNeuronio(ligacoes);
                }
            }

            for (int j = 0; j < qtdNeuroniosSaida; j++)
            {
                rede.CamadaSaida.Neuronios[j] = CriarNeuronio(qtdNeuroniosEscondida);
            }

            return rede;
        }

        public static void RNA_CopiarVetorParaCamadas(RedeNeural rede, double[] vetor)
        {
            int j = 0;
            for (int i = 0; i < rede.QuantidadeEscondidas; i++)
            {
                for (int k = 0; k < rede.CamadaEscondida[i].QuantidadeNeuronios; k++)
                {
                    for (int l = 0; l < rede.CamadaEscondida[i].Neuronios[k].QuantidadeLigacoes; l++)
                    {
                        rede.CamadaEscondida[i].Neuronios[k].Peso[l] = vetor[j++];
                    }
                }
            }

            for (int k = 0; k < rede.CamadaSaida.QuantidadeNeuronios; k++)
            {
                for (int l = 0; l < rede.CamadaSaida.Neuronios[k].QuantidadeLigacoes; l++)
                {
                    rede.CamadaSaida.Neuronios[k].Peso[l] = vetor[j++];
                }
            }
        }

        public static void RNA_CopiarParaEntrada(RedeNeural rede, double[] vetorEntrada)
        {
            for (int i = 0; i < rede.CamadaEntrada.QuantidadeNeuronios - BIAS; i++)
            {
                rede.CamadaEntrada.Neuronios[i].Saida = vetorEntrada[i];
            }
        }

        public static int RNA_QuantidadePesos(RedeNeural rede)
        {
            int soma = 0;
            for (int i = 0; i < rede.QuantidadeEscondidas; i++)
            {
                for (int j = 0; j < rede.CamadaEscondida[i].QuantidadeNeuronios; j++)
                {
                    soma += rede.CamadaEscondida[i].Neuronios[j].QuantidadeLigacoes;
                }
            }

            for (int i = 0; i < rede.CamadaSaida.QuantidadeNeuronios; i++)
            {
                soma += rede.CamadaSaida.Neuronios[i].QuantidadeLigacoes;
            }
            return soma;
        }

        public static void RNA_CopiarDaSaida(RedeNeural rede, double[] vetorSaida)
        {
            for (int i = 0; i < rede.CamadaSaida.QuantidadeNeuronios; i++)
            {
                vetorSaida[i] = rede.CamadaSaida.Neuronios[i].Saida;
            }
        }

        public static void RNA_CalcularSaida(RedeNeural rede)
        {
            double somatorio;
            for (int i = 0; i < rede.CamadaEscondida[0].QuantidadeNeuronios - BIAS; i++)
            {
                somatorio = 0;
                for (int j = 0; j < rede.CamadaEntrada.QuantidadeNeuronios; j++)
                {
                    somatorio += rede.CamadaEntrada.Neuronios[j].Saida * rede.CamadaEscondida[0].Neuronios[i].Peso[j];
                }
                rede.CamadaEscondida[0].Neuronios[i].Saida = AtivacaoOcultas(somatorio);
            }

            for (int k = 1; k < rede.QuantidadeEscondidas; k++)
            {
                for (int i = 0; i < rede.CamadaEscondida[k].QuantidadeNeuronios - BIAS; i++)
                {
                    somatorio = 0;
                    for (int j = 0; j < rede.CamadaEscondida[k - 1].QuantidadeNeuronios; j++)
                    {
                        somatorio += rede.CamadaEscondida[k - 1].Neuronios[j].Saida * rede.CamadaEscondida[k].Neuronios[i].Peso[j];
                    }
                    rede.CamadaEscondida[k].Neuronios[i].Saida = AtivacaoOcultas(somatorio);
                }
            }

            for (int i = 0; i < rede.CamadaSaida.QuantidadeNeuronios; i++)
            {
                somatorio = 0;
                int idx = rede.QuantidadeEscondidas - 1;
                for (int j = 0; j < rede.CamadaEscondida[idx].QuantidadeNeuronios; j++)
                {
                    somatorio += rede.CamadaEscondida[idx].Neuronios[j].Saida * rede.CamadaSaida.Neuronios[i].Peso[j];
                }
                rede.CamadaSaida.Neuronios[i].Saida = AtivacaoSaida(somatorio);
            }
        }

        public static RedeNeural RNA_DestruirRedeNeural(RedeNeural rede)
        {
            // Em C# o coletor de lixo lida com a liberação de memória
            return null;
        }

        public static RedeNeural RNA_CarregarRede(string caminho)
        {
            using var fs = File.OpenRead(caminho);
            using var br = new BinaryReader(fs);
            int qtdEscondida = br.ReadInt32();
            int qtdNeuroEntrada = br.ReadInt32();
            int qtdNeuroEscondida = br.ReadInt32();
            int qtdNeuroSaida = br.ReadInt32();

            var temp = RNA_CriarRedeNeural(qtdEscondida, qtdNeuroEntrada, qtdNeuroEscondida, qtdNeuroSaida);

            for (int k = 0; k < temp.QuantidadeEscondidas; k++)
            {
                for (int i = 0; i < temp.CamadaEscondida[k].QuantidadeNeuronios; i++)
                {
                    for (int j = 0; j < temp.CamadaEscondida[k].Neuronios[i].QuantidadeLigacoes; j++)
                    {
                        temp.CamadaEscondida[k].Neuronios[i].Peso[j] = br.ReadDouble();
                    }
                }
            }
            for (int i = 0; i < temp.CamadaSaida.QuantidadeNeuronios; i++)
            {
                for (int j = 0; j < temp.CamadaSaida.Neuronios[i].QuantidadeLigacoes; j++)
                {
                    temp.CamadaSaida.Neuronios[i].Peso[j] = br.ReadDouble();
                }
            }
            return temp;
        }

        public static void RNA_SalvarRede(RedeNeural temp, string caminho)
        {
            using var fs = File.Open(caminho, FileMode.Create);
            using var bw = new BinaryWriter(fs);
            bw.Write(temp.QuantidadeEscondidas);
            bw.Write(temp.CamadaEntrada.QuantidadeNeuronios);
            bw.Write(temp.CamadaEscondida[0].QuantidadeNeuronios);
            bw.Write(temp.CamadaSaida.QuantidadeNeuronios);

            for (int k = 0; k < temp.QuantidadeEscondidas; k++)
            {
                for (int i = 0; i < temp.CamadaEscondida[k].QuantidadeNeuronios; i++)
                {
                    for (int j = 0; j < temp.CamadaEscondida[k].Neuronios[i].QuantidadeLigacoes; j++)
                    {
                        bw.Write(temp.CamadaEscondida[k].Neuronios[i].Peso[j]);
                    }
                }
            }

            for (int i = 0; i < temp.CamadaSaida.QuantidadeNeuronios; i++)
            {
                for (int j = 0; j < temp.CamadaSaida.Neuronios[i].QuantidadeLigacoes; j++)
                {
                    bw.Write(temp.CamadaSaida.Neuronios[i].Peso[j]);
                }
            }
        }
    }
}
