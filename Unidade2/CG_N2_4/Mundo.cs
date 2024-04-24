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
    private Shader _shaderBranca;
    private Shader _shaderAmarela;
    private Shader _shaderCiano;

     public Mundo(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
               : base(gameWindowSettings, nativeWindowSettings)
    {
      mundo ??= new Objeto(null, ref rotuloAtual); 
    }

    protected override void OnLoad()
    {
      base.OnLoad();
      GL.ClearColor(0.5f, 0.5f, 0.5f, 1f);
      _vertexBufferObject_sruEixos = GL.GenBuffer();
      GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject_sruEixos);
      GL.BufferData<float>(BufferTarget.ArrayBuffer, _sruEixos.Length * 4, _sruEixos, BufferUsageHint.StaticDraw);
      _vertexArrayObject_sruEixos = GL.GenVertexArray();
      GL.BindVertexArray(_vertexArrayObject_sruEixos);
      GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 12, 0);
      GL.EnableVertexAttribArray(0);
      _shaderVermelha = new Shader("Shaders/shader.vert", "Shaders/shaderVermelha.frag");
      _shaderVerde = new Shader("Shaders/shader.vert", "Shaders/shaderVerde.frag");
      _shaderAzul = new Shader("Shaders/shader.vert", "Shaders/shaderAzul.frag");
      _shaderBranca = new Shader("Shaders/shader.vert", "Shaders/shaderBranca.frag");
      _shaderAmarela = new Shader("Shaders/shader.vert", "Shaders/shaderAmarela.frag");
      _shaderCiano = new Shader("Shaders/shader.vert", "Shaders/shaderCiano.frag");
      Spline spline = new Spline(Mundo.mundo, ref rotuloAtual);
      spline.ShaderObjeto = _shaderAmarela;
      objetoSelecionado = (Objeto)spline;
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
      base.OnUpdateFrame(e);
      KeyboardState keyboardState = KeyboardState;
      if (keyboardState.IsKeyPressed(Keys.Escape))
        Close();
      Spline objetoSelecionado = this.objetoSelecionado as Spline;
      if (keyboardState.IsKeyPressed(Keys.D))
        objetoSelecionado.AtualizarSpline(new Ponto4D(0.05));
      if (keyboardState.IsKeyPressed(Keys.E))
        objetoSelecionado.AtualizarSpline(new Ponto4D(-0.05));
      if (keyboardState.IsKeyPressed(Keys.C))
        objetoSelecionado.AtualizarSpline(new Ponto4D(y: 0.05));
      if (keyboardState.IsKeyPressed(Keys.B))
        objetoSelecionado.AtualizarSpline(new Ponto4D(y: -0.05));
      if (keyboardState.IsKeyPressed(Keys.Minus))
        objetoSelecionado.SplineQtdPto(-1);
      if (keyboardState.IsKeyPressed(Keys.Comma))
        objetoSelecionado.SplineQtdPto(1);
      if (!keyboardState.IsKeyPressed(Keys.Space))
        return;
      objetoSelecionado.AtualizarSpline(new Ponto4D(), true);
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
