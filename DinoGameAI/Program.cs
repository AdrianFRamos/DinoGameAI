using System;
using System.Threading;
using static System.Net.Mime.MediaTypeNames;
using static DinoGameAI.Tipos;
using static DinoGameAI.Game;
using static DinoGameAI.Variaveis;

namespace DinoGameAI
{
    internal class Game
    {
        public const int POPULACAO_TAMANHO = 100; // Defina o tamanho da população
        public const int LARG_GRAFICO = 100; // Defina o tamanho do gráfico
        public const int MODO_JOGO = 0; // Defina o modo de jogo

        public double[] DistanciaAtual = new double[POPULACAO_TAMANHO];
        private double[] DNADaVez = new double[POPULACAO_TAMANHO];
        public double[] BestFitnessPopulacao = new double[LARG_GRAFICO];
        public double[] MediaFitnessPopulacao = new double[LARG_GRAFICO];
        public int DinossaurosMortos;
        public int DistanciaRecorde;
        public int Geracao;
        public double VELOCIDADE = -3;
        public bool PIG_JogoRodando = true; // Controle do jogo

        public void DesenharThread() // Função chamada pela Thread responsável por desenhar na tela
        {
            while (jogoRodando)
            {
                Desenhar();
                Thread.Sleep(5); // Espera 5 milissegundos
            }
        }

        public void AplicarGravidade()
        {
            for (int i = 0; i < quantidadeDinossauros; i++)
            {
                if (dinossauros[i].Y > 15)
                {
                    if (dinossauros[i].Estado != 4) // VOANDO
                    {
                        dinossauros[i].VelocidadeY -= 0.08; // Aplica gravidade
                    }
                    else
                    {
                        dinossauros[i].VelocidadeY = Math.Max(dinossauros[i].VelocidadeY - 0.08, 0); // Mantém velocidade positiva
                    }

                    dinossauros[i].Y += dinossauros[i].VelocidadeY; // Atualiza a posição
                }
                else
                {
                    dinossauros[i].VelocidadeY = 0;
                    dinossauros[i].Y = 15; // Define a altura mínima
                    if (dinossauros[i].Estado == 2) // Se estava pulando
                        dinossauros[i].Estado = 0; // Volta para em pé
                }
            }
        }

        public void ControlarEstadoDinossauros() // Função responsável por calcular a decisão da rede neural e aplicar no dinossauro
        {
            int abaixar, pular, aviao;
            double[] saida = new double[10];
            double[] entrada = new double[10];

            for (int i = 0; i < quantidadeDinossauros; i++)
            {
                if (dinossauros[i].Estado != 3) // Se não está morto
                {
                    entrada[0] = DistanciaProximoObstaculo(dinossauros[i].X);
                    entrada[1] = LarguraProximoObstaculo(dinossauros[i].X);
                    entrada[2] = AlturaProximoObstaculo(dinossauros[i].X);
                    entrada[3] = ComprimentoProximoObstaculo(dinossauros[i].X);
                    entrada[4] = Math.Abs(VELOCIDADE);
                    entrada[5] = dinossauros[i].Y;

                    RNA_CopiarParaEntrada(dinossauros[i].Cerebro, entrada); // Enviando informações para a rede neural
                    RNA_CalcularSaida(dinossauros[i].Cerebro); // Calculando a decisão da rede
                    RNA_CopiarDaSaida(dinossauros[i].Cerebro, saida); // Extraindo a decisão

                    pular = (saida[0] == 0.0) ? 0 : 1;
                    abaixar = (saida[1] == 0.0) ? 0 : 1;
                    aviao = (saida[2] == 0.0) ? 0 : 1;

                    // Modos de controle
                    if (MODO_JOGO == 1 && i == 1)
                    {
                        pular = (PIG_meuTeclado[TECLA_CIMA] == 1) ? 1 : 0;
                        abaixar = (PIG_meuTeclado[TECLA_BAIXO] == 1) ? 1 : 0;
                        aviao = (PIG_meuTeclado[TECLA_BARRAESPACO] == 1) ? 1 : 0;
                    }

                    if (DINO_BRAIN_QTD_OUTPUT == 2)
                        aviao = 0;

                    if (dinossauros[i].Estado != 4) // Se não está voando
                    {
                        if (dinossauros[i].Estado != 2) // Se não está pulando
                            dinossauros[i].Estado = 0; // Em pé
                        if (abaixar == 1 && dinossauros[i].Estado != 2)
                            dinossauros[i].Estado = 1; // Deitado
                        if (abaixar == 1 && dinossauros[i].Estado == 2)
                        {
                            if (dinossauros[i].VelocidadeY > 0)
                                dinossauros[i].VelocidadeY = 0;
                            dinossauros[i].Y -= 2; // Desce um pouco
                        }
                        if (pular == 1 && dinossauros[i].Estado != 2)
                        {
                            dinossauros[i].Estado = 2; // Pulando
                            dinossauros[i].Y += 1;
                            dinossauros[i].VelocidadeY += 4.0; // Aumenta a velocidade para pular
                        }
                        if (aviao == 1 && dinossauros[i].AviaoCooldown <= 0)
                        {
                            dinossauros[i].Estado = 4; // Voando
                            dinossauros[i].Y += 1;
                            if (dinossauros[i].VelocidadeY <= 0.5 && dinossauros[i].Y < 25)
                                dinossauros[i].VelocidadeY += 4.0; // Aumenta a velocidade
                            dinossauros[i].AviaoCooldown = 4000.0; // Reinicia cooldown
                        }
                    }
                    else // Se está voando
                    {
                        if (dinossauros[i].AviaoDeslocamento >= 820.0)
                        {
                            dinossauros[i].AviaoDeslocamento = 0;
                            dinossauros[i].Estado = 2; // Volta para pulando
                        }
                        else
                        {
                            dinossauros[i].AviaoDeslocamento += Math.Abs(VELOCIDADE); // Aumenta deslocamento
                        }
                    }

                    dinossauros[i].AviaoCooldown -= Math.Abs(VELOCIDADE); // Diminui cooldown

                    // Atualiza sprite com base no estado
                    dinossauros[i].SpriteAtual = dinossauros[i].Estado switch
                    {
                        0 => 0 + dinossauros[i].Frame,
                        1 => 2 + dinossauros[i].Frame,
                        2 => 4 + dinossauros[i].Frame,
                        3 => 6 + dinossauros[i].Frame,
                        4 => 8 + dinossauros[i].Frame,
                        _ => dinossauros[i].SpriteAtual // Mantém o mesmo sprite se o estado não corresponder
                    };
                }
            }
        }

        private void InicializarNovaPartida()
        {
            GerarListaObstaculos();
            CarregarListaObstaculos();

            DistanciaAtual = 0;
            VELOCIDADE = -3;
            DinossaurosMortos = 0;

            InicializarObstaculos();

            for (int i = 0; i < POPULACAO_TAMANHO; i++)
            {
                InicializarDinossauro(i, DNADaVez[i], 300 + (new Random().Next(-100, 100)), 15);
            }
        }

        private void EncerrarPartida()
        {
            if (DistanciaAtual > DistanciaRecorde)
            {
                DistanciaRecorde = (int)DistanciaAtual;
                SalvarRedeArquivo();
            }
        }

        private void CarregarRede()
        {
            using (var fs = new FileStream("rede", FileMode.Open, FileAccess.Read))
            {
                using (var reader = new BinaryReader(fs))
                {
                    int tamanhoDNA = reader.ReadInt32();
                    for (int i = 0; i < tamanhoDNA; i++)
                    {
                        DNADaVez[i] = reader.ReadDouble();
                    }
                }
            }
        }

        private void ConfiguracoesIniciais()
        {
            CriarJanela("Google Dinosaur", 0);
            InicializarSprites();

            InicializarChao();
            InicializarMontanhas();
            InicializarNuvens();

            AlocarDinossauros();
            AlocarObstaculos();
            CarregarListaObstaculos();
            InicializarGrafico();

            TimerGeral = CriarTimer();
            Fonte = CriarFonteNormal("..\\fontes\\arial.ttf", 15, PRETO, 0, PRETO);
            FonteVermelha = CriarFonteNormal("..\\fontes\\arial.ttf", 15, VERMELHO, 0, PRETO);
            FonteAzul = CriarFonteNormal("..\\fontes\\arial.ttf", 15, AZUL, 0, PRETO);
            DistanciaRecorde = 0;
            Geracao = 0;

            InicializarDNA();
            InicializarNovaPartida();
        }

        private void RandomMutations()
        {
            static double RangeRandom = Dinossauros[0].TamanhoDNA;

            Dinossauro[] Vetor = new Dinossauro[POPULACAO_TAMANHO];
            Dinossauro Temp;

            if (Geracao < LARG_GRAFICO)
            {
                GeracaoCompleta = Geracao + 1;
                BestFitnessPopulacao[Geracao] = BestFitnessGeracao();
                MediaFitnessPopulacao[Geracao] = MediaFitnessGeracao();
            }
            else
            {
                for (int i = 0; i < LARG_GRAFICO - 1; i++)
                {
                    BestFitnessPopulacao[i] = BestFitnessPopulacao[i + 1];
                    MediaFitnessPopulacao[i] = MediaFitnessPopulacao[i + 1];
                }
                BestFitnessPopulacao[GeracaoCompleta] = BestFitnessGeracao();
                MediaFitnessPopulacao[GeracaoCompleta] = MediaFitnessGeracao();
            }

            for (int i = 0; i < POPULACAO_TAMANHO; i++)
            {
                Vetor[i] = Dinossauros[i];
            }

            Array.Sort(Vetor, (a, b) => b.Fitness.CompareTo(a.Fitness));

            int Step = 1;
            for (int i = 0; i < Step; i++)
            {
                for (int j = Step + i; j < POPULACAO_TAMANHO; j += Step)
                {
                    for (int k = 0; k < Vetor[j].TamanhoDNA; k++)
                    {
                        Vetor[j].DNA[k] = Vetor[i].DNA[k];
                    }
                }
            }

            for (int j = Step; j < POPULACAO_TAMANHO; j++)
            {
                int tipo;
                int mutations = new Random().Next(1, (int)RangeRandom + 1);

                for (int k = 0; k < mutations; k++)
                {
                    tipo = new Random().Next(0, 3);
                    int indice = new Random().Next(0, Vetor[j].TamanhoDNA);
                    switch (tipo)
                    {
                        case 0:
                            Vetor[j].DNA[indice] = GetRandomValue();
                            break;
                        case 1:
                            double number = new Random().Next(5000, 10001) / 10000.0 + 0.5;
                            Vetor[j].DNA[indice] *= number;
                            break;
                        case 2:
                            double numberSum = GetRandomValue() / 100.0;
                            Vetor[j].DNA[indice] += numberSum;
                            break;
                    }
                }
            }

            for (int j = 0; j < POPULACAO_TAMANHO; j++)
            {
                for (int k = 0; k < Dinossauros[j].TamanhoDNA; k++)
                {
                    DNADaVez[j][k] = Dinossauros[j].DNA[k];
                }
            }

            for (int i = 0; i < POPULACAO_TAMANHO; i++)
            {
                Vetor[i].ResetarFitness = 1;
            }

            Console.WriteLine($"Range Random: {RangeRandom}");
            RangeRandom *= 0.99;
            if (RangeRandom < 20)
            {
                RangeRandom = 20;
            }

            Geracao++;
        }

        private void VerificarFimDePartida()
        {
            if (DinossaurosMortos == POPULACAO_TAMANHO)
            {
                EncerrarPartida();
                if (MODO_JOGO == 0)
                {
                    RandomMutations();
                }
                InicializarNovaPartida();
            }
        }

        static void Main(string[] args)
        {
            Game game = new Game();
            game.ConfiguracoesIniciais();

            Thread desenharThread = new Thread(game.DesenharThread);
            desenharThread.Start();

            while (game.PIG_JogoRodando)
            {
                game.AtualizarJanela();
                game.VerificarTeclas();

                if (game.TempoDecorrido(game.TimerGeral) >= game.Periodo)
                {
                    game.MovimentarChao();
                    game.MovimentarMontanhas();
                    game.MovimentarNuvem();
                    game.MovimentarObstaculos();
                    game.MovimentarDinossauros();

                    game.AtualizarFramePassaro();
                    game.AtualizarFrameDinossauro();
                    game.AtualizarFrameAviao();
                    game.AtualizarMelhorDinossauro();
                    game.AplicarGravidade();
                    game.AplicarColisao();
                    game.ControlarEstadoDinossauros();

                    if (Math.Abs(game.VELOCIDADE) < 8)
                    {
                        game.VELOCIDADE -= 0.0005;
                    }

                    game.DistanciaAtual += Math.Abs(game.VELOCIDADE);
                    if (game.DistanciaAtual > 1000000 && game.DistanciaAtual > game.DistanciaRecorde)
                    {
                        // game.SalvarRedeArquivo();
                        game.DinossaurosMortos = POPULACAO_TAMANHO;
                    }

                    game.VerificarFimDePartida();
                    game.ReiniciarTimer(game.TimerGeral);
                }
            }
            game.FinalizarJanela();
        }
    }
}
