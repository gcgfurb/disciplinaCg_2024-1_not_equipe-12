using CG_Biblioteca;
using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;

namespace gcgcg
{
  internal class Poligono : Objeto
  {
    public Poligono(Objeto _paiRef, ref char _rotulo, List<Ponto4D> pontosPoligono)
      : base(_paiRef, ref _rotulo)
    {
      this.PrimitivaTipo = PrimitiveType.LineLoop;
      this.PrimitivaTamanho = 1f;
      this.pontosLista = pontosPoligono;
      this.Atualizar();
    }

    private void Atualizar() => this.ObjetoAtualizar();

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
