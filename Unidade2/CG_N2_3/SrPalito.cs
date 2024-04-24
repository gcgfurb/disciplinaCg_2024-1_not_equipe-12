#define CG_Debug

using CG_Biblioteca;
using OpenTK.Graphics.OpenGL4;

namespace gcgcg
{
  internal class SrPalito : Objeto
  {
    public double Angulo { get; set; }
    public double Raio { get; set;}
    public SrPalito(Objeto _paiRef, ref char _rotulo) : this(_paiRef, ref _rotulo, new Ponto4D(0.0, 0.0), 45.0, 0.5) {

    }

    public SrPalito(Objeto _paiRef, ref char _rotulo, Ponto4D ptoIni, double angulo, double raio) : base(_paiRef, ref _rotulo)
    {
      Angulo = angulo;
      Raio = raio;
      PrimitivaTipo = PrimitiveType.Lines;
      PrimitivaTamanho = 0.5f;
      Ponto4D ptoFin = Matematica.GerarPtosCirculo(Angulo, raio);
      ptoFin.X += ptoIni.X;
      ptoFin.Y += ptoIni.Y;
      base.PontosAdicionar(ptoIni);
      base.PontosAdicionar(ptoFin);
      Atualizar();
    }

    public void AlterarTamanho(double tamanho){
      Raio += tamanho;
      PontosAlterar(Matematica.GerarPtosCirculo(Angulo, Raio), 1);
      Atualizar();
    }

    public void Girar(double angulo){
      Angulo += angulo;
      PontosAlterar(Matematica.GerarPtosCirculo(Angulo, Raio), 1);
      Atualizar();
    }

    private void Atualizar()
    {

      base.ObjetoAtualizar();
    }
  }
}
