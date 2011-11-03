using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL;
using _3DScanner.Interoperability;

namespace _3DScanner.GLMeshConstructor
{
    public class SinglePerspectiveReconstructor
    {
        public static readonly int maxSteps = 5;			// maximum steps allowed until separation of structures, in depth as well as from left to right (good for filling gaps)
        //public readonly int samples = 5;			// count of subsamples to interpolate depth (1 = off)

        public static void PerspectiefNaarEasyMesh(IList<Perspective> perspectives)
        {
            int counter = 0;
            LOG.Instance.publishMessage("START CREATING ONE MESH PER PERSPECTIVE: " + perspectives.Count + " PERSPECTIVES");
            IEnumerator<Perspective> e = perspectives.GetEnumerator();
            while (e.MoveNext())
            {
                LOG.Instance.publishMessage("Start creating mesh " + ++counter + " out of" + perspectives.Count + " meshes");
                lock (e.Current)
                {
                    int i, k, x, y, depthPos,j;
                    float xf, yf, zf;
                    int[] vertIdx = new int[e.Current.XRes * e.Current.YRes];
                    for (i=0,j=0; i<640*480; i++) {
		                depthPos = (int)e.Current.Depth[i];
		                if (depthPos>340 && depthPos<1081) j++;
	                }
                    Vertex[] vertices = new Vertex[j];
                    IList<Face> faces = new List<Face>();
            
                    // apply noise filter
                    // perform smoothing filter to remove noise (use iterations here to remove low frequency noise better)
                    int colorIndex = 0;
                    for (y = 0, i = 0, j = 0; y < e.Current.XRes; y++)
                        for (x = 0; x < e.Current.YRes; x++, i++)
                        {
                            //j = (y * e.Current.YRes + x) * 3;
                            // numbers seems to be in ranges:
                            // extrema 18,1120
                            // 2 rooms +floor (~13m) 1064
                            // 1 room +floor (~7m) 1045
                            // 6.0m 1038
                            // 1.7m 875
                            // 1.0m 765
                            // 0.6m 456
                            // 0.5m 370 minimum measureable distance
                            depthPos = (int)e.Current.Depth[i];
                            if (depthPos > 380 && depthPos < 1081)
                            {
                                colorIndex = (y * 640 + x) * 3;
                                xf = (float)x;
                                yf = (float)y;
                                zf = (float)e.Current.Depth[i];
                                zf = -(1 / ((zf - 1091) / 355)); // convert depth to meters, see diagrams for details... 351=0.48m, 1080=32m, 1090=355m
                                xf = (((xf - 320) / 320) * zf * .5f); // perspective correction for X displacement of 57° FOV 
                                yf = ((yf - 240) / 240) * zf * .4f; // perspective correction for Y TODO correction still needed
                                if (float.IsNaN(xf) || float.IsInfinity(xf) || xf < -100 || xf > 100) xf = 0;
                                if (float.IsNaN(yf) || float.IsInfinity(yf) || yf < -100 || yf > 100) yf = 0;
                                if (float.IsNaN(zf) || float.IsInfinity(zf) || zf < -100 || zf > 100) zf = 0;
                                // store vertex coordinates]
                                vertices[j] = new Vertex();
                                vertices[j].Position = new Vector3(xf, -yf, -zf);
                                vertices[j].RGBB = e.Current.CopyPixels()[y,x].Blue;
                                vertices[j].RGBG = e.Current.CopyPixels()[y, x].Green;
                                vertices[j].RGBR = e.Current.CopyPixels()[y, x].Red;
                                /*vertex[(y * 640 + x) * 3 + 0] = ;
                                vertex[(y * 640 + x) * 3 + 1] = -yf;
                                vertex[(y * 640 + x) * 3 + 2] = -zf;*/
                                //vertIdx[i] = j;	// store indices for further processing
                                j++;
                            }
                        }

                    /*if (useFilter != 0)
                    {
                        // (float factor, int iterations, float surfacePreserve, short radius, short shape, short fallOff)
                        filterVertices(0.5f, 2, 0, 0, 0, 0);
                        filterVertices(0.5f, 20, 1.0f, 8, 1, 1);
                        filterVertices(0.5f, 20, 1.0f, 8, 2, 1);
                        filterVertices(0.5f, 20, 0.5f, 2, 1, 3);
                        filterVertices(0.5f, 20, 0.5f, 2, 2, 3);
                        //		filterVertices(.95, 50, .05, 2, 0, 3);
                    }*/

                    //for (int ii = 0; ii < rgb.Length; ii++) { rgb[ii] = 100; }//TODO vervangen voor echte kleurwaarden
                    // save vertices to file

                    // save faces
                    for (y = 1; y < e.Current.YRes; y++)
                    {
                        for (x = 1; x < e.Current.XRes; x++)
                        {
                            depthPos = (int)e.Current.Depth[y * e.Current.XRes + x];
                            if (depthPos > 340 && depthPos < 1081)
                            {
                                k = depthPos;
                                depthPos = (int)e.Current.Depth[(y - 1) * 640 + (x - 1)];
                                if (depthPos > 340 && depthPos < 1081 && depthPos > k - maxSteps && depthPos < k + maxSteps)
                                {
                                    k = depthPos;
                                    depthPos = (int)e.Current.Depth[y * 640 + x - 1];
                                    if (depthPos > 340 && depthPos < 1081 && depthPos > k - maxSteps && depthPos < k + maxSteps)
                                    {
                                        k = depthPos;
                                        depthPos = (int)e.Current.Depth[(y - 1) * 640 + x];
                                        if (depthPos > 340 && depthPos < 1081 && depthPos > k - maxSteps && depthPos < k + maxSteps)
                                        {
                                            Quad q = new Quad();
                                            q.Point1 = vertices[vertIdx[(y - 1) * 640 + (x - 1)]];
                                            q.Point2 = vertices[vertIdx[y * 640 + x - 1]];
                                            q.Point3 = vertices[vertIdx[y * 640 + x]];
                                            q.Point4 = vertices[vertIdx[(y - 1) * 640 + x]];
                                            faces.Add(q);
                                            //tw.Write("4 {0:d} {0:d} {0:d} {0:d}\n", );	// save a quad if possible, but those will be reduced to triangles by opengl. Still ply doesn't do that
                                        }
                                        else
                                        {
                                            Triangle t = new Triangle();
                                            t.Point1 = vertices[vertIdx[y * 640 + x]];
                                            t.Point2 = vertices[vertIdx[(y - 1) * 640 + (x - 1)]];
                                            t.Point3 = vertices[vertIdx[y * 640 + x - 1]];
                                            faces.Add(t);
                                            //mesh.Faces.Add(new Triangel(
                                            //    mesh.vertices[vertIdx[y * 640 + x]],
                                            //    mesh.vertices[vertIdx[(y - 1) * 640 + (x - 1)]],
                                            //    mesh.vertices[vertIdx[y * 640 + x - 1]]));
                                            //tw.Write("3 {0:d} {0:d} {0:d}\n",); // in case there are not enough vertices for a quad save a triangle
                                        }
                                    }
                                    else
                                    {
                                        depthPos = (int)e.Current.Depth[(y - 1) * 640 + x];
                                        if (depthPos > 340 && depthPos < 1081 && depthPos > k - maxSteps && depthPos < k + maxSteps)
                                        {
                                            Triangle t = new Triangle();
                                            t.Point1 = vertices[vertIdx[(y - 1) * 640 + (x - 1)]];
                                            t.Point2 = vertices[vertIdx[y * 640 + x]];
                                            t.Point3 = vertices[vertIdx[(y - 1) * 640 + x]];
                                            faces.Add(t);
                                            //mesh.Faces.Add(
                                            //new Triangel(
                                            //    mesh.Vertices[vertIdx[(y - 1) * 640 + (x - 1)]],
                                            //    mesh.Vertices[vertIdx[y * 640 + x]],
                                            //    mesh.Vertices[vertIdx[(y - 1) * 640 + x]])); // in case there are not enough vertices for a quad save a triangle
                                        }
                                    }
                                }
                            }
                        }
                    }
                    BeeldOpslag.Instance.registerMesh(new Mesh(faces, vertices),e.Current);
                }
            }
            LOG.Instance.publishMessage("START CREATING ONE MESH PER PERSPECTIVE: DONE");
        }
    }
}
