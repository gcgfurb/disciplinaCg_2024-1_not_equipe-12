#define CG_Debug

using CG_Biblioteca;
using OpenTK.Graphics.OpenGL4;

namespace gcgcg
{
  internal class Circulo : Objeto
  {
    private readonly double raio;

    public Circulo(Objeto _paiRef, ref char _rotulo, double _raio)
      : this(_paiRef, ref _rotulo, _raio, new Ponto4D())
    {
    }

    public Circulo(Objeto _paiRef, ref char _rotulo, double _raio, Ponto4D ptoDeslocamento)
      : base(_paiRef, ref _rotulo)
    {
      raio = _raio;
      PrimitivaTipo = PrimitiveType.Points;
      PrimitivaTamanho = 5f;
      for (int angulo = 0; angulo < 360; angulo += 5)
        PontosAdicionar(Matematica.GerarPtosCirculo((double)angulo, raio) + ptoDeslocamento);
      ObjetoAtualizar();
    }

    public void Atualizar(Ponto4D ptoDeslocamento)
    {
      int posicao = 0;
      for (int angulo = 0; angulo < 360; angulo += 5)
      {
        PontosAlterar(Matematica.GerarPtosCirculo((double)angulo, raio) + ptoDeslocamento, posicao);
        ++posicao;
      }
      ObjetoAtualizar();
    }

#if CG_Debug
    public override string ToString()
    {
      string retorno;
      retorno = "__ Objeto Ponto _ Tipo: " + PrimitivaTipo + " _ Tamanho: " + PrimitivaTamanho + "\n";
      retorno += base.ImprimeToString();
      return retorno;
    }
#endif
  }
}
