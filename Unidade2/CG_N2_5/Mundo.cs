//TODO: testar se estes DEFINEs continuam funcionado
#define CG_Gizmo  // debugar gráfico.
#define CG_OpenGL // render OpenGL.
// #define CG_DirectX // render DirectX.
// #define CG_Privado // código do professor.

using CG_Biblioteca;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;


namespace gcgcg
{
  public class Mundo : GameWindow
  {
    private static Objeto mundo;
    private char rotuloAtual = '?';
    private Objeto objetoSelecionado;
    private readonly float[] _sruEixos =
        {
            -0.5f,  0.0f,  0.0f, /* X- */      0.5f,  0.0f,  0.0f, /* X+ */
            0.0f, -0.5f,  0.0f, /* Y- */      0.0f,  0.5f,  0.0f, /* Y+ */
            0.0f,  0.0f, -0.5f, /* Z- */      0.0f,  0.0f,  0.5f, /* Z+ */
        };
    private int _vertexBufferObject_sruEixos;
    private int _vertexArrayObject_sruEixos;
    private Shader _shaderVermelha;
    private Shader _shaderVerde;
    private Shader _shaderAzul;
    private Ponto ptoCental;
    private Retangulo bbox;
    private Circulo circulo;
    private bool mouse = true;
    private Ponto4D mousePto;

    public Mundo(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
               : base(gameWindowSettings, nativeWindowSettings)
    {
      mundo ??= new Objeto(null, ref rotuloAtual); 
    }

    protected override void OnLoad()
    {
      base.OnLoad(); // Chama o método da classe base

      // Configura a cor de fundo para preto
      GL.ClearColor(0.0f, 0.0f, 0.0f, 1f);

      // Gera um buffer de vértices para os eixos
      _vertexBufferObject_sruEixos = GL.GenBuffer();
      GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject_sruEixos);
      GL.BufferData<float>(BufferTarget.ArrayBuffer, _sruEixos.Length * 4, _sruEixos, BufferUsageHint.StaticDraw);

      // Gera um array de vértices para os eixos
      _vertexArrayObject_sruEixos = GL.GenVertexArray();
      GL.BindVertexArray(_vertexArrayObject_sruEixos);
      GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 12, 0);
      GL.EnableVertexAttribArray(0);

      // Carrega os shaders para as cores vermelha, verde e azul
      _shaderVermelha = new Shader("Shaders/shader.vert", "Shaders/shaderVermelha.frag");
      _shaderVerde = new Shader("Shaders/shader.vert", "Shaders/shaderVerde.frag");
      _shaderAzul = new Shader("Shaders/shader.vert", "Shaders/shaderAzul.frag");

      // Cria um ponto central para o objeto
      Ponto ponto = new Ponto(Mundo.mundo, ref rotuloAtual, new Ponto4D(0.3, 0.3));
      ponto.PrimitivaTipo = PrimitiveType.Points;
      ponto.PrimitivaTamanho = 10f;
      ptoCental = ponto;

      // Cria um círculo menor
      Circulo circulo1 = new Circulo(Mundo.mundo, ref rotuloAtual, 0.1, ptoCental.PontosId(0));
      circulo1.PrimitivaTipo = PrimitiveType.LineLoop;
      circulo = circulo1;

      // Cria um círculo maior como objeto selecionado
      Circulo circulo2 = new Circulo(Mundo.mundo, ref rotuloAtual, 0.3, ptoCental.PontosId(0));
      circulo2.PrimitivaTipo = PrimitiveType.LineLoop;
      objetoSelecionado = (Objeto)circulo2;

      // Cria um retângulo delimitador interno
      double num = Matematica.GerarPtosCirculoSimetrico(0.3);
      Ponto4D ponto4D1 = new Ponto4D(num, num);
      Ponto4D ponto4D2 = new Ponto4D(-num, -num);
      Ponto4D pto1 = ponto4D1 + ptoCental.PontosId(0);
      Ponto4D pto2 = ponto4D2 + ptoCental.PontosId(0);
      Retangulo retangulo = new Retangulo(Mundo.mundo, ref rotuloAtual, pto2, pto1);
      retangulo.PrimitivaTipo = PrimitiveType.LineLoop;
      bbox = retangulo;
    }


    protected override void OnRenderFrame(FrameEventArgs e)
    {
      base.OnRenderFrame(e);
      GL.Clear(ClearBufferMask.ColorBufferBit);
      Sru3D();
      Mundo.mundo.Desenhar();
      SwapBuffers();
    }

    protected override void OnUpdateFrame(FrameEventArgs e)
    {
      base.OnUpdateFrame(e); // Chama o método da classe base para atualizar o frame

      Ponto4D pto = ptoCental.PontosId(0); // Obtém o ponto central do objeto
      double variacaoX1 = 0.01; // Define o incremento no eixo X
      double variacaoY1 = 0.01; // Define o incremento no eixo Y
      KeyboardState keyboardState = KeyboardState; // Obtém o estado do teclado

      // Verifica se a tecla Escape foi pressionada e fecha a janela
      if (keyboardState.IsKeyPressed(Keys.Escape))
        Close();

      // Verifica se a tecla P foi pressionada e imprime o objeto selecionado no console
      if (keyboardState.IsKeyPressed(Keys.P))
        Console.WriteLine((object)objetoSelecionado);

      // Verifica se a tecla D foi pressionada e testa a caixa delimitadora do objeto
      if (keyboardState.IsKeyPressed(Keys.D))
        BBox(pto, variacaoX1, 0.0);

      // Verifica se a tecla E foi pressionada e testa a caixa delimitadora do objeto
      if (keyboardState.IsKeyPressed(Keys.E))
        BBox(pto, -variacaoX1, 0.0);

      // Verifica se a tecla C foi pressionada e testa a caixa delimitadora do objeto
      if (keyboardState.IsKeyPressed(Keys.C))
        BBox(pto, 0.0, variacaoY1);

      // Verifica se a tecla B foi pressionada e testa a caixa delimitadora do objeto
      if (keyboardState.IsKeyPressed(Keys.B))
        BBox(pto, 0.0, -variacaoY1);

      int x = Size.X; // Obtém a largura da janela
      int y = Size.Y; // Obtém a altura da janela
      Ponto4D ponto4D1 = new Ponto4D((double)MousePosition.X, (double)MousePosition.Y); // Obtém a posição do mouse
      int altura = y; // Define a altura da janela
      Ponto4D mousePosition = ponto4D1; // Obtém a posição do mouse em relação à janela
      Ponto4D ponto4D2 = Utilitario.NDC_TelaSRU(x, altura, mousePosition); // Converte a posição do mouse

      // Verifica se a tecla Shift esquerda está pressionada
      if (!keyboardState.IsKeyDown(Keys.LeftShift))
        return; // Se não estiver, retorna

      // Se for o primeiro movimento do mouse
      if (mouse)
      {
        mousePto = ponto4D2; // Define o último ponto do movimento do mouse
        mouse = false; // Define a flag de primeiro movimento como falsa
      }
      else // Se não for o primeiro movimento do mouse
      {
        double variacaoX2 = ponto4D2.X - mousePto.X; // Calcula o incremento no eixo X
        double variacaoY2 = ponto4D2.Y - mousePto.Y; // Calcula o incremento no eixo Y
        mousePto = ponto4D2; // Atualiza o último ponto do movimento do mouse
        BBox(pto, variacaoX2, variacaoY2); // Testa a caixa delimitadora do objeto
        Atualizar(pto); // Atualiza o objeto
      }
    }

    private void BBox(Ponto4D pto, double incX, double incY)
    {
      pto.X += incX; // Incrementa a coordenada X do ponto
      pto.Y += incY; // Incrementa a coordenada Y do ponto

      // Verifica se o ponto está dentro da caixa delimitadora interna
      if (Matematica.Dentro(bbox.Bbox(), pto))
      {
        bbox.PrimitivaTipo = PrimitiveType.LineLoop; // Define o tipo de primitiva da caixa delimitadora interna
        Atualizar(pto); // Atualiza o objeto com a nova posição do ponto
      }
      else
      {
        bbox.PrimitivaTipo = PrimitiveType.Points; // Define o tipo de primitiva da caixa delimitadora interna como pontos

        // Verifica se o ponto está dentro do círculo menor
        if (Matematica.distanciaQuadrado(pto, new Ponto4D(0.3, 0.3)) <= 0.09)
        {
          Atualizar(pto); // Atualiza o objeto com a nova posição do ponto
        }
        else
        {
          pto.X -= incX; // Desfaz o incremento da coordenada X do ponto
          pto.Y -= incY; // Desfaz o incremento da coordenada Y do ponto
        }
      }
    }

    private void Atualizar(Ponto4D pto)
    {
      ptoCental.PontosAlterar(pto, 0); // Atualiza o ponto central do objeto com a nova posição
      ptoCental.Atualizar(); // Atualiza o objeto
      circulo.Atualizar(pto); // Atualiza o círculo menor com a nova posição do ponto
    }
    protected override void OnResize(ResizeEventArgs e)
    {
      base.OnResize(e);
      GL.Viewport(0, 0, Size.X, Size.Y);
    }

    protected override void OnUnload()
    {
      Mundo.mundo.OnUnload();
      GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
      GL.BindVertexArray(0);
      GL.UseProgram(0);
      GL.DeleteBuffer(_vertexBufferObject_sruEixos);
      GL.DeleteVertexArray(_vertexArrayObject_sruEixos);
      GL.DeleteProgram(_shaderVermelha.Handle);
      GL.DeleteProgram(_shaderVerde.Handle);
      GL.DeleteProgram(_shaderAzul.Handle);
      base.OnUnload();
    }

    private void Sru3D()
    {
      GL.BindVertexArray(_vertexArrayObject_sruEixos);
      _shaderVermelha.Use();
      GL.DrawArrays(PrimitiveType.Lines, 0, 2);
      _shaderVerde.Use();
      GL.DrawArrays(PrimitiveType.Lines, 2, 2);
      _shaderAzul.Use();
      GL.DrawArrays(PrimitiveType.Lines, 4, 2);
    }
  }
}
