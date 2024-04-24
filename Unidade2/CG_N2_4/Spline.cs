

using CG_Biblioteca;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;

namespace gcgcg
{
  internal class Spline : Objeto
  {
    private int pto;
    private readonly Ponto[] ptos = new Ponto[4];
    private Poligono solido;
    private readonly int splineQtdPtoMax = 10;
    private int splineQtdPto = 10;
    private readonly double[,] bezier = new double[11, 4];

    // Método para atualizar o número de pontos
    public void SplineQtdPto(int inc)
    {
      // Incrementa ou decrementa o número de pontos dentro dos limites
      splineQtdPto = (inc >= 0 || splineQtdPto <= 0) ? splineQtdPto : (splineQtdPto += inc);
      splineQtdPto = (inc <= 0 || splineQtdPto >= splineQtdPtoMax) ? splineQtdPto : (splineQtdPto += inc);

      // Calcula os coeficientes de Bezier e atualiza a spline
      Bezier();
      Atualizar();
    }


    // Método para calcular os coeficientes de Bezier
    private void Bezier()
    {
      int index = 0;
      for (double i = 0.0; i <= 1.0; i += 1.0 / (double)splineQtdPto)
      {
        // Calcula os coeficientes de Bezier para cada valor do parâmetro
        bezier[index, 0] = Math.Pow(1.0 - i, 3.0);
        bezier[index, 1] = 3.0 * i * Math.Pow(1.0 - i, 2.0);
        bezier[index, 2] = 3.0 * Math.Pow(i, 2.0) * (1.0 - i);
        bezier[index, 3] = Math.Pow(i, 3.0);
        ++index;
      }
    }

    // Construtor da classe Spline
    public Spline(Objeto _paiRef, ref char _rotulo)
      : base(_paiRef, ref _rotulo)
    {
      Ptos(); // Inicializa os pontos
      Solido(); // Inicializa a representação sólida
      PrimitivaTipo = PrimitiveType.LineStrip; // Define o tipo primitivo para renderização
      Bezier(); // Calcula os coeficientes de Bezier iniciais
      for (int index = 0; index <= splineQtdPto; ++index)
        PontosAdicionar(new Ponto4D()); // Adiciona pontos para renderizar a spline
      Atualizar(); // Atualiza a spline
    }

    // Método para inicializar os pontos da spline
    private void Ptos()
    {
      char _rotulo = 'S';
      // Cria e adiciona os pontos à spline
      ptos[0] = new Ponto((Objeto)this, ref _rotulo, new Ponto4D(0.5, -0.5));
      FilhoAdicionar((Objeto)ptos[0]);
      ptos[0].ObjetoAtualizar();
      ptos[1] = new Ponto((Objeto)this, ref _rotulo, new Ponto4D(0.5, 0.5));
      FilhoAdicionar((Objeto)ptos[1]);
      ptos[1].ObjetoAtualizar();
      ptos[2] = new Ponto((Objeto)this, ref _rotulo, new Ponto4D(-0.5, 0.5));
      FilhoAdicionar((Objeto)ptos[2]);
      ptos[2].ObjetoAtualizar();
      ptos[3] = new Ponto((Objeto)this, ref _rotulo, new Ponto4D(-0.5, -0.5));
      FilhoAdicionar((Objeto)ptos[3]);
      ptos[3].ObjetoAtualizar();
    }


    // Método para inicializar a representação sólida da spline
    private void Solido()
    {
      char _rotulo = 'S';
      List<Ponto4D> pontosPoligono = new List<Ponto4D>();

      // Adiciona os pontos à lista de pontos do polígono
      pontosPoligono.Add(ptos[0].PontosId(0));
      pontosPoligono.Add(ptos[1].PontosId(0));
      pontosPoligono.Add(ptos[2].PontosId(0));
      pontosPoligono.Add(ptos[3].PontosId(0));

      // Cria o polígono com os pontos da lista
      Poligono poligono = new Poligono((Objeto)this, ref _rotulo, pontosPoligono);
      poligono.PrimitivaTipo = PrimitiveType.LineStrip;
      poligono.ShaderObjeto = new Shader("Shaders/shader.vert", "Shaders/shaderCiano.frag");
      solido = poligono;

      // Adiciona o polígono como filho do objeto atual
      FilhoAdicionar((Objeto)solido);
      solido.ObjetoAtualizar();

    }

    // Método para atualizar a curva spline com base nos pontos
    public void Atualizar()
    {
      ptos[pto].ShaderObjeto = new Shader("Shaders/shader.vert", "Shaders/shaderVermelha.frag");
      Ponto4D ponto4D1 = new Ponto4D(ptos[0].PontosId(0));
      Ponto4D ponto4D2 = new Ponto4D(ptos[1].PontosId(0));
      Ponto4D ponto4D3 = new Ponto4D(ptos[2].PontosId(0));
      Ponto4D ponto4D4 = new Ponto4D(ptos[3].PontosId(0));
      for (int posicao = 0; posicao <= splineQtdPto; ++posicao)
        PontosAlterar(new Ponto4D(ponto4D1.X * bezier[posicao, 0] + ponto4D2.X * bezier[posicao, 1] + ponto4D3.X * bezier[posicao, 2] + ponto4D4.X * bezier[posicao, 3], ponto4D1.Y * bezier[posicao, 0] + ponto4D2.Y * bezier[posicao, 1] + ponto4D3.Y * bezier[posicao, 2] + ponto4D4.Y * bezier[posicao, 3]), posicao);
      ObjetoAtualizar();
    }

    // Método para atualizar a curva spline com base em alterações nas posições dos pontos
    public void AtualizarSpline(Ponto4D ptoInc, bool proximo = false)
    {
      ptos[pto].ShaderObjeto = new Shader("Shaders/shader.vert", "Shaders/shaderBranca.frag");
      if (proximo)
        pto = pto >= 3 ? 0 : ++pto;
      ptos[pto].ShaderObjeto = new Shader("Shaders/shader.vert", "Shaders/shaderVermelha.frag");
      ptos[pto].PontosAlterar(ptos[pto].PontosId(0) + ptoInc, 0);
      ptos[pto].ObjetoAtualizar();
      solido.PontosAlterar(ptos[pto].PontosId(0), pto);
      solido.ObjetoAtualizar();
      Atualizar();
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
