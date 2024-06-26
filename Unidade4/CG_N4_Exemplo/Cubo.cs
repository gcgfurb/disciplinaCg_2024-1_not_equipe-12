//https://github.com/mono/opentk/blob/main/Source/Examples/Shapes/Old/Cube.cs

#define CG_Debug
using CG_Biblioteca;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Drawing;

namespace gcgcg
{
  internal class Cubo : Objeto
  {
    Ponto4D[] vertices;
    // int[] indices;
    // Vector3[] normals;
    // int[] colors;

    public Cubo(Objeto _paiRef, ref char _rotulo) : base(_paiRef, ref _rotulo)
    {
PrimitivaTipo = PrimitiveType.TriangleStrip;
PrimitivaTamanho = 14; // Número total de vértices usados no strip

vertices = new Ponto4D[]
{
    // Frente
    new Ponto4D(-1.0f, -1.0f,  1.0f), // 0
    new Ponto4D( 1.0f, -1.0f,  1.0f), // 1
    new Ponto4D(-1.0f,  1.0f,  1.0f), // 2
    new Ponto4D( 1.0f,  1.0f,  1.0f), // 3

    // Direita
    new Ponto4D( 1.0f, -1.0f,  1.0f), // 1
    new Ponto4D( 1.0f, -1.0f, -1.0f), // 4
    new Ponto4D( 1.0f,  1.0f,  1.0f), // 3
    new Ponto4D( 1.0f,  1.0f, -1.0f), // 6

    // Trás
    new Ponto4D( 1.0f, -1.0f, -1.0f), // 4
    new Ponto4D(-1.0f, -1.0f, -1.0f), // 5
    new Ponto4D( 1.0f,  1.0f, -1.0f), // 6
    new Ponto4D(-1.0f,  1.0f, -1.0f), // 7

    // Esquerda
    new Ponto4D(-1.0f, -1.0f, -1.0f), // 5
    new Ponto4D(-1.0f, -1.0f,  1.0f), // 0
    new Ponto4D(-1.0f,  1.0f, -1.0f), // 7
    new Ponto4D(-1.0f,  1.0f,  1.0f), // 2

    // Topo
    new Ponto4D(-1.0f,  1.0f,  1.0f), // 2
    new Ponto4D( 1.0f,  1.0f,  1.0f), // 3
    new Ponto4D(-1.0f,  1.0f, -1.0f), // 7
    new Ponto4D( 1.0f,  1.0f, -1.0f), // 6

    // Fundo
    new Ponto4D(-1.0f, -1.0f, -1.0f), // 5
    new Ponto4D( 1.0f, -1.0f, -1.0f), // 4
    new Ponto4D(-1.0f, -1.0f,  1.0f), // 0
    new Ponto4D( 1.0f, -1.0f,  1.0f)  // 1
};

foreach (var ponto in vertices)
{
    base.PontosAdicionar(ponto);
}
      Atualizar();
    }

    private void Atualizar()
    {

      base.ObjetoAtualizar();
    }

#if CG_Debug
    public override string ToString()
    {
      string retorno;
      retorno = "__ Objeto Cubo _ Tipo: " + PrimitivaTipo + " _ Tamanho: " + PrimitivaTamanho + "\n";
      retorno += base.ImprimeToString();
      return (retorno);
    }
#endif

  }
}
