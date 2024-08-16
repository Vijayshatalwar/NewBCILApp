using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Media3D;
using System.Windows.Media.Animation;
using System.IO;

namespace GREENPLY
{
    /// <summary>
    /// Interaction logic for Logo_Cube.xaml
    /// </summary>
    public partial class Logo_Cube : UserControl
    {
        public Logo_Cube()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            DrawMeshes();
            DrawSomeModels();
        }


        #region Logo Animation

        #region Variables

        Viewport3D myViewport = new Viewport3D();
        GeometryModel3D side1 = new GeometryModel3D();
        GeometryModel3D side2 = new GeometryModel3D();
        GeometryModel3D side3 = new GeometryModel3D();
        GeometryModel3D side4 = new GeometryModel3D();
        GeometryModel3D side5 = new GeometryModel3D();
        GeometryModel3D side6 = new GeometryModel3D();
        PerspectiveCamera myPCamera = new PerspectiveCamera();
        DirectionalLight myDLight = new DirectionalLight();
        AmbientLight myAmbLight = new AmbientLight();
        MaterialGroup myMaterials = new MaterialGroup();
        Transform3DGroup cube1TransformGroup = new Transform3DGroup();

        Transform3DGroup allModelsTransformGroup = new Transform3DGroup();
        Model3DGroup allModels = new Model3DGroup();
        Model3DGroup cubeModel_1 = new Model3DGroup();

        MeshGeometry3D side1Plane = new MeshGeometry3D();
        MeshGeometry3D side2Plane = new MeshGeometry3D();
        MeshGeometry3D side3Plane = new MeshGeometry3D();
        MeshGeometry3D side4Plane = new MeshGeometry3D();
        MeshGeometry3D side5Plane = new MeshGeometry3D();
        MeshGeometry3D side6Plane = new MeshGeometry3D();
        DiffuseMaterial side1Material = new DiffuseMaterial();
        DiffuseMaterial side2Material = new DiffuseMaterial();
        DiffuseMaterial side3Material = new DiffuseMaterial();
        DiffuseMaterial side4Material = new DiffuseMaterial();
        DiffuseMaterial side5Material = new DiffuseMaterial();
        DiffuseMaterial side6Material = new DiffuseMaterial();

        #endregion

        private void DrawSomeModels()
        {
            myViewport.Name = "myViewport";
            ModelVisual3D myModelVisual = new ModelVisual3D();

            //Define lights and cameras
            myPCamera.FarPlaneDistance = 20;
            myPCamera.NearPlaneDistance = 0;
            myPCamera.FieldOfView = 50;
            myPCamera.Position = new Point3D(-5, 2, 3);
            myPCamera.LookDirection = new Vector3D(5, -2, -3);
            myPCamera.UpDirection = new Vector3D(0, 1, 0);

            myDLight.Color = Colors.White;
            myDLight.Direction = new Vector3D(-3, -4, -5);

            myAmbLight.Color = Colors.White;

            //set Geometry property of MeshGeometry3D
            side2.Geometry = side2Plane;
            side6.Geometry = side6Plane;
            side1.Geometry = side1Plane;
            side3.Geometry = side3Plane;
            side4.Geometry = side4Plane;
            side5.Geometry = side5Plane;

            //create translations
            TranslateTransform3D cube2Translation = new TranslateTransform3D(new Vector3D(2, 0, 0));
            TranslateTransform3D cube3Translation = new TranslateTransform3D(new Vector3D(4, 0, 0));
            //Define a rotation
            RotateTransform3D myRotateTransform = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), 1));
            //Define an animation for the rotation
            DoubleAnimation myAnimation = new DoubleAnimation();
            myAnimation.From = 1;
            myAnimation.To = 361;
            myAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(7000));
            myAnimation.RepeatBehavior = RepeatBehavior.Forever;

            //Define another animation
            Vector3DAnimation myVectorAnimation = new Vector3DAnimation(new Vector3D(-1, -1, -1), new Duration(TimeSpan.FromMilliseconds(5000)));
            myVectorAnimation.RepeatBehavior = RepeatBehavior.Forever;


            myRotateTransform.Rotation.BeginAnimation(AxisAngleRotation3D.AngleProperty, myAnimation);
            myRotateTransform.Rotation.BeginAnimation(AxisAngleRotation3D.AxisProperty, myVectorAnimation);
            //Add transformation to the model
            cube1TransformGroup.Children.Add(myRotateTransform);


            allModelsTransformGroup.Children.Add(myRotateTransform);

            cubeModel_1.Children.Add(side1);
            cubeModel_1.Children.Add(side2);
            cubeModel_1.Children.Add(side3);
            cubeModel_1.Children.Add(side4);
            cubeModel_1.Children.Add(side5);
            cubeModel_1.Children.Add(side6);
            cubeModel_1.Transform = cube1TransformGroup;

            allModels.Transform = allModelsTransformGroup;

            allModels.Children.Add(cubeModel_1);

            allModels.Children.Add(myAmbLight);

            myViewport.Camera = myPCamera;
            myModelVisual.Content = allModels;
            myViewport.Children.Add(myModelVisual);

            Logo.Content = myViewport;

        }

        private void DrawMeshes()
        {
            //side1-------------------------------------------------
            side1Plane.Positions.Add(new Point3D(-0.4, -0.4, -0.4));
            side1Plane.Positions.Add(new Point3D(-0.4, 0.4, -0.4));
            side1Plane.Positions.Add(new Point3D(0.4, 0.4, -0.4));
            side1Plane.Positions.Add(new Point3D(0.4, 0.4, -0.4));
            side1Plane.Positions.Add(new Point3D(0.4, -0.4, -0.4));
            side1Plane.Positions.Add(new Point3D(-0.4, -0.4, -0.4));

            side1Plane.TriangleIndices.Add(0);
            side1Plane.TriangleIndices.Add(1);
            side1Plane.TriangleIndices.Add(2);
            side1Plane.TriangleIndices.Add(3);
            side1Plane.TriangleIndices.Add(4);
            side1Plane.TriangleIndices.Add(5);

            side1Plane.Normals.Add(new Vector3D(0, 0, -1));
            side1Plane.Normals.Add(new Vector3D(0, 0, -1));
            side1Plane.Normals.Add(new Vector3D(0, 0, -1));
            side1Plane.Normals.Add(new Vector3D(0, 0, -1));
            side1Plane.Normals.Add(new Vector3D(0, 0, -1));
            side1Plane.Normals.Add(new Vector3D(0, 0, -1));

            side1Plane.TextureCoordinates.Add(new Point(1, 0));
            side1Plane.TextureCoordinates.Add(new Point(0, 0));
            side1Plane.TextureCoordinates.Add(new Point(0, 1));
            side1Plane.TextureCoordinates.Add(new Point(0, 1));
            side1Plane.TextureCoordinates.Add(new Point(1, 1));
            side1Plane.TextureCoordinates.Add(new Point(1, 0));

            //side2-------------------------------------------------
            side2Plane.Positions.Add(new Point3D(-0.4, -0.4, 0.4));
            side2Plane.Positions.Add(new Point3D(0.4, -0.4, 0.4));
            side2Plane.Positions.Add(new Point3D(0.4, 0.4, 0.4));
            side2Plane.Positions.Add(new Point3D(0.4, 0.4, 0.4));
            side2Plane.Positions.Add(new Point3D(-0.4, 0.4, 0.4));
            side2Plane.Positions.Add(new Point3D(-0.4, -0.4, 0.4));

            side2Plane.TriangleIndices.Add(0);
            side2Plane.TriangleIndices.Add(1);
            side2Plane.TriangleIndices.Add(2);
            side2Plane.TriangleIndices.Add(3);
            side2Plane.TriangleIndices.Add(4);
            side2Plane.TriangleIndices.Add(5);

            side2Plane.Normals.Add(new Vector3D(0, 0, 1));
            side2Plane.Normals.Add(new Vector3D(0, 0, 1));
            side2Plane.Normals.Add(new Vector3D(0, 0, 1));
            side2Plane.Normals.Add(new Vector3D(0, 0, 1));
            side2Plane.Normals.Add(new Vector3D(0, 0, 1));
            side2Plane.Normals.Add(new Vector3D(0, 0, 1));

            side2Plane.TextureCoordinates.Add(new Point(0, 0));
            side2Plane.TextureCoordinates.Add(new Point(0, 1));
            side2Plane.TextureCoordinates.Add(new Point(1, 1));
            side2Plane.TextureCoordinates.Add(new Point(1, 1));
            side2Plane.TextureCoordinates.Add(new Point(1, 0));
            side2Plane.TextureCoordinates.Add(new Point(0, 0));

            //side3-------------------------------------------------
            side3Plane.Positions.Add(new Point3D(-0.4, -0.4, -0.4));
            side3Plane.Positions.Add(new Point3D(0.4, -0.4, -0.4));
            side3Plane.Positions.Add(new Point3D(0.4, -0.4, 0.4));
            side3Plane.Positions.Add(new Point3D(0.4, -0.4, 0.4));
            side3Plane.Positions.Add(new Point3D(-0.4, -0.4, 0.4));
            side3Plane.Positions.Add(new Point3D(-0.4, -0.4, -0.4));

            side3Plane.TriangleIndices.Add(0);
            side3Plane.TriangleIndices.Add(1);
            side3Plane.TriangleIndices.Add(2);
            side3Plane.TriangleIndices.Add(3);
            side3Plane.TriangleIndices.Add(4);
            side3Plane.TriangleIndices.Add(5);

            side3Plane.Normals.Add(new Vector3D(0, -1, 0));
            side3Plane.Normals.Add(new Vector3D(0, -1, 0));
            side3Plane.Normals.Add(new Vector3D(0, -1, 0));
            side3Plane.Normals.Add(new Vector3D(0, -1, 0));
            side3Plane.Normals.Add(new Vector3D(0, -1, 0));
            side3Plane.Normals.Add(new Vector3D(0, -1, 0));

            side3Plane.TextureCoordinates.Add(new Point(0, 0));
            side3Plane.TextureCoordinates.Add(new Point(0, 1));
            side3Plane.TextureCoordinates.Add(new Point(1, 1));
            side3Plane.TextureCoordinates.Add(new Point(1, 1));
            side3Plane.TextureCoordinates.Add(new Point(1, 0));
            side3Plane.TextureCoordinates.Add(new Point(0, 0));

            //side4-------------------------------------------------
            side4Plane.Positions.Add(new Point3D(0.4, -0.4, -0.4));
            side4Plane.Positions.Add(new Point3D(0.4, 0.4, -0.4));
            side4Plane.Positions.Add(new Point3D(0.4, 0.4, 0.4));
            side4Plane.Positions.Add(new Point3D(0.4, 0.4, 0.4));
            side4Plane.Positions.Add(new Point3D(0.4, -0.4, 0.4));
            side4Plane.Positions.Add(new Point3D(0.4, -0.4, -0.4));

            side4Plane.TriangleIndices.Add(0);
            side4Plane.TriangleIndices.Add(1);
            side4Plane.TriangleIndices.Add(2);
            side4Plane.TriangleIndices.Add(3);
            side4Plane.TriangleIndices.Add(4);
            side4Plane.TriangleIndices.Add(5);

            side4Plane.Normals.Add(new Vector3D(1, 0, 0));
            side4Plane.Normals.Add(new Vector3D(1, 0, 0));
            side4Plane.Normals.Add(new Vector3D(1, 0, 0));
            side4Plane.Normals.Add(new Vector3D(1, 0, 0));
            side4Plane.Normals.Add(new Vector3D(1, 0, 0));
            side4Plane.Normals.Add(new Vector3D(1, 0, 0));

            side4Plane.TextureCoordinates.Add(new Point(1, 0));
            side4Plane.TextureCoordinates.Add(new Point(1, 1));
            side4Plane.TextureCoordinates.Add(new Point(0, 1));
            side4Plane.TextureCoordinates.Add(new Point(0, 1));
            side4Plane.TextureCoordinates.Add(new Point(0, 0));
            side4Plane.TextureCoordinates.Add(new Point(1, 0));

            //side5-------------------------------------------------
            side5Plane.Positions.Add(new Point3D(0.4, 0.4, -0.4));
            side5Plane.Positions.Add(new Point3D(-0.4, 0.4, -0.4));
            side5Plane.Positions.Add(new Point3D(-0.4, 0.4, 0.4));
            side5Plane.Positions.Add(new Point3D(-0.4, 0.4, 0.4));
            side5Plane.Positions.Add(new Point3D(0.4, 0.4, 0.4));
            side5Plane.Positions.Add(new Point3D(0.4, 0.4, -0.4));

            side5Plane.TriangleIndices.Add(0);
            side5Plane.TriangleIndices.Add(1);
            side5Plane.TriangleIndices.Add(2);
            side5Plane.TriangleIndices.Add(3);
            side5Plane.TriangleIndices.Add(4);
            side5Plane.TriangleIndices.Add(5);

            side5Plane.Normals.Add(new Vector3D(0, 1, 0));
            side5Plane.Normals.Add(new Vector3D(0, 1, 0));
            side5Plane.Normals.Add(new Vector3D(0, 1, 0));
            side5Plane.Normals.Add(new Vector3D(0, 1, 0));
            side5Plane.Normals.Add(new Vector3D(0, 1, 0));
            side5Plane.Normals.Add(new Vector3D(0, 1, 0));

            side5Plane.TextureCoordinates.Add(new Point(1, 1));
            side5Plane.TextureCoordinates.Add(new Point(0, 1));
            side5Plane.TextureCoordinates.Add(new Point(0, 0));
            side5Plane.TextureCoordinates.Add(new Point(0, 0));
            side5Plane.TextureCoordinates.Add(new Point(1, 0));
            side5Plane.TextureCoordinates.Add(new Point(1, 1));

            //side6-------------------------------------------------
            side6Plane.Positions.Add(new Point3D(-0.4, 0.4, -0.4));
            side6Plane.Positions.Add(new Point3D(-0.4, -0.4, -0.4));
            side6Plane.Positions.Add(new Point3D(-0.4, -0.4, 0.4));
            side6Plane.Positions.Add(new Point3D(-0.4, -0.4, 0.4));
            side6Plane.Positions.Add(new Point3D(-0.4, 0.4, 0.4));
            side6Plane.Positions.Add(new Point3D(-0.4, 0.4, -0.4));

            side6Plane.TriangleIndices.Add(0);
            side6Plane.TriangleIndices.Add(1);
            side6Plane.TriangleIndices.Add(2);
            side6Plane.TriangleIndices.Add(3);
            side6Plane.TriangleIndices.Add(4);
            side6Plane.TriangleIndices.Add(5);

            side6Plane.Normals.Add(new Vector3D(-1, 0, 0));
            side6Plane.Normals.Add(new Vector3D(-1, 0, 0));
            side6Plane.Normals.Add(new Vector3D(-1, 0, 0));
            side6Plane.Normals.Add(new Vector3D(-1, 0, 0));
            side6Plane.Normals.Add(new Vector3D(-1, 0, 0));
            side6Plane.Normals.Add(new Vector3D(-1, 0, 0));

            side6Plane.TextureCoordinates.Add(new Point(0, 1));
            side6Plane.TextureCoordinates.Add(new Point(0, 0));
            side6Plane.TextureCoordinates.Add(new Point(1, 0));
            side6Plane.TextureCoordinates.Add(new Point(1, 0));
            side6Plane.TextureCoordinates.Add(new Point(1, 1));
            side6Plane.TextureCoordinates.Add(new Point(0, 1));

            BitmapImage ImgTest = new BitmapImage(new Uri(System.Windows.Navigation.BaseUriHelper.GetBaseUri(this), @"\Images\shivamlogonew.jpg")); 


            ImageBrush imgBrush = new ImageBrush(ImgTest);


            //Set Brush property for the Material applied to each face
            DiffuseMaterial side2Material = new DiffuseMaterial(imgBrush);

            DiffuseMaterial side6Material = new DiffuseMaterial(imgBrush);

            //DiffuseMaterial side1Material = new DiffuseMaterial((Brush)Application.Current.Resources["gradientBrush"]);

            DiffuseMaterial side1Material = new DiffuseMaterial(imgBrush);
            DiffuseMaterial side3Material = new DiffuseMaterial(imgBrush);
            DiffuseMaterial side4Material = new DiffuseMaterial(imgBrush);
            //DiffuseMaterial side5Material = new DiffuseMaterial((Brush)Application.Current.Resources["purpleBrush"]);
            DiffuseMaterial side5Material = new DiffuseMaterial(imgBrush);


            side2.Material = side2Material;
            side6.Material = side6Material;
            side1.Material = side1Material;
            side3.Material = side3Material;
            side4.Material = side4Material;
            side5.Material = side5Material;


        }


        #endregion

    }
}
