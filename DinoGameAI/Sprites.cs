using System;
using System.Collections.Generic;
using System.Drawing; 
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DinoGameAI
{
    internal class Sprites
    {
        const int OBSTACULOS_SPRITE_QUANTIDADE = 8;

        public struct Sprite
        {
            public int Largura;
            public int Altura;
            public int Objeto;
        }

        // Declaração das variáveis globais
        public Sprite[] SpriteObstaculo = new Sprite[OBSTACULOS_SPRITE_QUANTIDADE];
        public Sprite[] SpriteAviao = new Sprite[2];

        public int SpriteNeuronDesativado;
        public int SpriteNeuronAtivado;
        public int SpriteLuz;
        public int SpriteSeta;

        public int CriarSprite(string caminhoImagem)
        {
            try
            {
                Image imagem = Image.FromFile(caminhoImagem);

                Random random = new Random();
                int identificadorSprite = random.Next(1, 1000);

                Console.WriteLine($"Imagem carregada: {caminhoImagem} - ID do Sprite: {identificadorSprite}");
                return identificadorSprite;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar a imagem: {caminhoImagem}. Detalhes: {ex.Message}");
                return -1;
            }
        }

        public void InicializarSpriteAviao()
        {
            string[] strings = new string[2];

            for (int i = 0; i < 2; i++)
            {
                strings[i] = $"imagens\\aviao{i}.bmp";
                SpriteAviao[i].Objeto = CriarSprite(strings[i]);
                SpriteAviao[i].Largura = 70;
                SpriteAviao[i].Altura = 37;
            }
        }

        public void InicializarSpritesObstaculos()
        {
            int[] LarguraObstaculos = { 32, 23, 15, 49, 73, 42, 42, 810 };
            int[] AlturaObstaculos = { 33, 46, 33, 33, 47, 36, 36, 31 };

            for (int i = 0; i < OBSTACULOS_SPRITE_QUANTIDADE - 1; i++)
            {
                string caminhoImagem = $"imagens\\obs{i}.bmp";
                SpriteObstaculo[i].Objeto = CriarSprite(caminhoImagem);
                SpriteObstaculo[i].Largura = LarguraObstaculos[i];
                SpriteObstaculo[i].Altura = AlturaObstaculos[i];
            }

            // Para a última imagem
            SpriteObstaculo[OBSTACULOS_SPRITE_QUANTIDADE - 1].Objeto = CriarSprite("imagens\\obs7.png");
            SpriteObstaculo[OBSTACULOS_SPRITE_QUANTIDADE - 1].Largura = LarguraObstaculos[OBSTACULOS_SPRITE_QUANTIDADE - 1];
            SpriteObstaculo[OBSTACULOS_SPRITE_QUANTIDADE - 1].Altura = AlturaObstaculos[OBSTACULOS_SPRITE_QUANTIDADE - 1];
        }

        public void InicializarSprites()
        {
            InicializarSpritesObstaculos();
            InicializarSpriteAviao();

            SpriteNeuronDesativado = CriarSprite("imagens//neuronio7.png");
            DefinirColoracao(SpriteNeuronDesativado, PRETO);

            SpriteNeuronAtivado = CriarSprite("imagens//neuronio7.png");
            DefinirColoracao(SpriteNeuronAtivado, BRANCO);

            SpriteLuz = CriarSprite("imagens//luz.png");

            SpriteSeta = CriarSprite("imagens\\seta2.png");
            DefinirColoracao(SpriteSeta, PRETO);
        }

        public Sprite GetMontanhaSprite(int codigo)
        {
            Sprite sprite = new Sprite
            {
                Largura = 1366,
                Altura = 180
            };

            switch (codigo)
            {
                case 1:
                    sprite.Objeto = CriarSprite("imagens\\fundo0.bmp");
                    break;
                case 2:
                    sprite.Objeto = CriarSprite("imagens\\fundo1.bmp");
                    break;
                case 11:
                    sprite.Objeto = CriarSprite("imagens\\fundo2.bmp");
                    break;
                case 12:
                    sprite.Objeto = CriarSprite("imagens\\fundo3.bmp");
                    break;
                case 21:
                    sprite.Objeto = CriarSprite("imagens\\fundo4.bmp");
                    break;
                case 22:
                    sprite.Objeto = CriarSprite("imagens\\fundo5.bmp");
                    break;
            }

            return sprite;
        }

        public Sprite GetChaoSprite()
        {
            int cont = 0;
            int indice;
            Sprite sprite;

            if (cont == 4)
            {
                indice = new Random().Next(4, 6); // Gera um número aleatório entre 4 e 5
                cont = 0;
            }
            else
            {
                indice = new Random().Next(0, 4); // Gera um número aleatório entre 0 e 3
                cont++;
            }

            string caminhoImagem = $"imagens\\chao{indice}.bmp";
            sprite.Objeto = CriarSprite(caminhoImagem);
            sprite.Largura = 60;
            sprite.Altura = 12;

            return sprite;
        }

        public Sprite GetDinossauroSprite(int indice, PIG_Cor cor)
        {
            int[] larguraFramesDino = { 40, 40, 55, 55, 40, 40, 40, 40, 39, 39 };
            int[] alturaFramesDino = { 43, 43, 25, 26, 43, 43, 43, 43, 37, 37 };
            Sprite sprite;

            string caminhoImagem = $"imagens\\dino{indice}.bmp";
            sprite.Objeto = CriarSprite(caminhoImagem);

            sprite.Largura = larguraFramesDino[indice];
            sprite.Altura = alturaFramesDino[indice];

            DefinirColoracao(sprite.Objeto, cor);

            return sprite;
        }

        public Sprite GetObstaculosSprite(int tipo, int frame)
        {
            if (tipo == PASSARO_CODIGO_TIPO)
            {
                return frame == 0 ? SpriteObstaculo[5] : SpriteObstaculo[6];
            }
            else if (tipo == ESPINHO_CODIGO_TIPO)
            {
                return SpriteObstaculo[7];
            }
            else
            {
                return SpriteObstaculo[tipo];
            }
        }

        public Sprite GetNuvemSprite()
        {
            Sprite sprite;

            sprite.Objeto = CriarSprite("imagens\\nuvem.bmp");
            sprite.Largura = 46;
            sprite.Altura = 13;

            return sprite;
        }
    }
}
