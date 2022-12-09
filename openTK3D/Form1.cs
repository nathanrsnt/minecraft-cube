using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace openTK3D
{
    public partial class OpenTK3D : Form
    {
        int texturaLateral;
        int texturaCima;
        int lateral = 0;

        Vector3d dir = new Vector3d(0, -450, 120);        //direção da câmera
        Vector3d pos = new Vector3d(0, -550, 120);     //posiçãoo da câmera
        float camera_rotation = 0;                     //rotação no eixo Z

        public OpenTK3D()
        {
            InitializeComponent();
        }

        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit); //limpa os buffers
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity(); //zera a matriz de projeção com a matriz identidade

            Matrix4d lookat = Matrix4d.LookAt(pos.X, pos.Y, pos.Z, dir.X, dir.Y, dir.Z, 0, 0, 1);

            //aplica a transformacao na matriz de rotacao
            GL.LoadMatrix(ref lookat);

            GL.Enable(EnableCap.DepthTest);

            //EIXOS X, Y, Z
            GL.Begin(PrimitiveType.Lines);
              GL.Color3(Color.Red);
              GL.Vertex3(0, 0, 0); GL.Vertex3(500, 0, 0);
              GL.Color3(Color.Blue);
              GL.Vertex3(0, 0, 0); GL.Vertex3(0, 500, 0);
              GL.Color3(Color.Green);
              GL.Vertex3(0, 0, 0); GL.Vertex3(0, 0, 500);
            GL.End();

            GL.Color3(Color.Transparent);
            //LADO 1
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, texturaLateral);
            GL.Begin(PrimitiveType.Quads);  
            GL.TexCoord2(1, 0); GL.Vertex3(100, 100, 100);
            GL.TexCoord2(1, 1); GL.Vertex3(100, 100, -100);
            GL.TexCoord2(0, 1); GL.Vertex3(-100, 100, -100);
            GL.TexCoord2(0, 0); GL.Vertex3(-100, 100, 100);
            GL.End();
            GL.Disable(EnableCap.Texture2D);
            //LADO 2   
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, texturaLateral);
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(1, 0); GL.Vertex3(-100,-100,100);
            GL.TexCoord2(1, 1); GL.Vertex3(-100,-100,-100);
            GL.TexCoord2(0, 1); GL.Vertex3(100,-100,-100);
            GL.TexCoord2(0, 0); GL.Vertex3(100,-100,100);
            GL.End();
            GL.Disable(EnableCap.Texture2D);
            //LADO 3
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, texturaLateral);
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(0, 0); GL.Vertex3(100, 100, 100);
            GL.TexCoord2(1, 0); GL.Vertex3(100, -100, 100);
            GL.TexCoord2(1, 1); GL.Vertex3(100, -100, -100);
            GL.TexCoord2(0, 1);  GL.Vertex3(100, 100, -100);
            GL.End();
            GL.Disable(EnableCap.Texture2D);
            //LADO 4
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, texturaLateral);
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(0, 0); GL.Vertex3(-100, 100, 100);
            GL.TexCoord2(1, 0); GL.Vertex3(-100, -100, 100);
            GL.TexCoord2(1, 1); GL.Vertex3(-100, -100, -100);
            GL.TexCoord2(0, 1); GL.Vertex3(-100, 100, -100);
            GL.End();
            GL.Disable(EnableCap.Texture2D);
            //CIMA
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, texturaCima);
            GL.Begin(PrimitiveType.Quads);
            GL.Vertex3(100, 100, 100);
            GL.Vertex3(100, -100, 100);
            GL.Vertex3(-100, -100, 100);
            GL.Vertex3(-100, 100, 100);
            GL.End();
            GL.Disable(EnableCap.Texture2D);
            //BAIXO
            GL.Begin(PrimitiveType.Quads);
            GL.Vertex3(100, 100, -100);
            GL.Vertex3(100, -100, -100);
            GL.Vertex3(-100, -100, -100);
            GL.Vertex3(-100, 100, -100);
            GL.End();

            glControl1.SwapBuffers(); //troca os buffers de frente e de fundo 

        }
        private void glControl1_Load(object sender, EventArgs e)
        {
            texturaLateral = LoadTexture("../../img/lado.jpg"); //carrega as texturas laterais
            texturaCima = LoadTexture("../../img/cima.jpg"); //carrega a textura superior

            GL.ClearColor(Color.Black);         // definindo a cor de limpeza do fundo da tela
            GL.Enable(EnableCap.Light0);

            SetupViewport();                      //configura a janela de pintura
        }

        private void SetupViewport() //configura a janela de projeção 
        {
            int w = glControl1.Width; //largura da janela
            int h = glControl1.Height; //altura da janela

            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(1.0f, w / (float)h, 1.0f, 2000.0f);


            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity(); //zera a matriz de projeção com a matriz identidade

            GL.LoadMatrix(ref projection);

            GL.Viewport(0, 0, w, h); // usa toda area de pintura da glcontrol
            lateral = w / 2;

        }


        private void calcula_direcao()
        {
            dir.X = pos.X + (Math.Sin(camera_rotation * Math.PI / 180) * 100);
            dir.Y = pos.Y + (Math.Cos(camera_rotation * Math.PI / 180) * 100);
        }
        private void glControl1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.X > lateral)
            {
                camera_rotation += 2;
            }
            if (e.X < lateral)
            {
                camera_rotation -= 2;
            }
            lateral = e.X;
            calcula_direcao();
            glControl1.Invalidate();
        }

        private void glControl1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            float a = camera_rotation;
            int tipoTecla = 0;
            if (e.KeyCode == Keys.Left)
            {
                a -= 90;
                tipoTecla = 1;
            }
            if (e.KeyCode == Keys.Right)
            {
                a += 90;
                tipoTecla = 1;
            }
            if (e.KeyCode == Keys.Up)
            { tipoTecla = 1; }
            if (e.KeyCode == Keys.Down)
            {
                a += 180;
                tipoTecla = 1;
            }

            if (e.KeyCode == Keys.D)
            {
                a += 1;
                tipoTecla = 2;
            }
            if (e.KeyCode == Keys.A)
            {
                a -= 1;
                tipoTecla = 2;
            }
            if (tipoTecla == 1)
            {
                if (a < 0) a += 360;
                if (a > 360) a -= 360;
                pos.X += (Math.Sin(a * Math.PI / 180) * 10);
                pos.Y += (Math.Cos(a * Math.PI / 180) * 10);
                calcula_direcao();
                glControl1.Invalidate();
            }

            if (tipoTecla == 2)
            {
                camera_rotation = a;
                calcula_direcao();
                glControl1.Invalidate();
            }
        }

        static int LoadTexture(string filename)
        {
            if (String.IsNullOrEmpty(filename))
                throw new ArgumentException(filename);

            int id;//= GL.GenTexture(); 

            GL.GenTextures(1, out id);
            GL.BindTexture(TextureTarget.Texture2D, id);

            Bitmap bmp = new Bitmap(filename);

            BitmapData data = bmp.LockBits(new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height),
                ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            bmp.UnlockBits(data);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            return id;
        }
    }
}
