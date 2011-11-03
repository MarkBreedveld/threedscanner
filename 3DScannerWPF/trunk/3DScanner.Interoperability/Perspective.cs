using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using OpenNI;
using System.Windows;
using System.Runtime.InteropServices;

//Het engelse gedeelte is code van kai
//Maar omdat de code kris kras door de applicatie verdwijnt
//Zal niet meer te herkennen zijn wat van hem is of nog maar te zeggen dat het van hem is
//Toch ter ere van zijn werk noteren we zijn naam Kai.
namespace _3DScanner.Interoperability
{
    public class Perspective
    {
        private Mesh _Mesh;
        internal Mesh Mesh
        {
            get { return _Mesh; }
            set { _Mesh = value; }
        }

        private int[] Histogram { get; set; }
        private double HAngle;
        private double VAngle;
        private readonly DateTime createTime;

        public DateTime CreateTime
        {
            get { return createTime; }
        }

        /// <summary>
        /// Horizontal bitmap dpi.
        /// </summary>
        private readonly int DPI_X = 96;

        /// <summary>
        /// Vertical bitmap dpi.
        /// </summary>
        private readonly int DPI_Y = 96;

        public int XRes
        {
            get
            {
                if (_imgMD.XRes == _depthMD.XRes && _imgMD.YRes == _depthMD.YRes)
                {
                    return _imgMD.XRes;
                }
                throw new InvalidOperationException("The sizes of the depth and image do not match.");
            }
        }
        public int YRes
        {
            get { return _imgMD.YRes; }
        }

        private bool isfinal = false;
        public bool Isfinal
        {
            get { return isfinal; }
            private set { isfinal = value; }
        }

        /// <summary>
        /// Image camera source.
        /// </summary>
        private WriteableBitmap _imageBitmap;

        private WriteableBitmap _depthBitmap;
        public ImageSource DepthBitmap
        {
            get
            {
                if (_depthBitmap == null)
                {
                    _depthBitmap = new WriteableBitmap(_depthMD.XRes, _depthMD.YRes, DPI_X, DPI_Y, PixelFormats.Rgb24, null);
                    Histogram = new int[_depthMD.XRes * _depthMD.YRes];
                    UpdateHistogram(_depthMD);

                    _depthBitmap.Lock();

                    unsafe
                    {
                        ushort* pDepth = (ushort*)DepthMD.DepthMapPtr.ToPointer();
                        for (int y = 0; y < _depthMD.YRes; ++y)
                        {
                            byte* pDest = (byte*)_depthBitmap.BackBuffer.ToPointer() + y * _depthBitmap.BackBufferStride;
                            for (int x = 0; x < _depthMD.XRes; ++x, ++pDepth, pDest += 3)
                            {
                                byte pixel = (byte)Histogram[*pDepth];
                                pDest[0] = 0;
                                pDest[1] = pixel;
                                pDest[2] = pixel;
                            }
                        }
                    }

                    _depthBitmap.AddDirtyRect(new Int32Rect(0, 0, _depthMD.XRes, _depthMD.YRes));
                    _depthBitmap.Unlock();
                }

                return _depthBitmap;
            }
        }
        /// <summary>
        /// Raw image metadata.
        /// </summary>
        private ImageMetaData _imgMD = new ImageMetaData();
        public ImageMetaData ImgMD
        {
            get { return _imgMD; }
            /*set
            {
                if (value == null) { throw new ArgumentNullException("value was null"); }
                if (isfinal) { throw new ArgumentException("The perspective is finalized. So it should not be changed."); }
                if (this.XRes == 0 && this.YRes == 0)
                {
                    if (value.XRes > 0)
                    {
                        this.XRes = value.XRes;
                        this.YRes = value.YRes;
                        _imgMD = value;
                    }
                    else{throw new ArgumentException("Empty metadata object with no resolution.");}
                }
                else
                {
                    if (this.XRes == value.XRes && this.YRes == value.YRes)
                    {
                        _imgMD = value;
                        this.Isfinal = true;
                    }
                    else { throw new ArgumentException("Image resolution didn't match the depth resolution."); }
                }
            }*/
        }

        /// <summary>
        /// Depth image metadata.
        /// TODO Should protect the object internals
        /// </summary>
        private DepthMetaData _depthMD = new DepthMetaData();
        public DepthMetaData DepthMD
        {
            get { return _depthMD; }
            /*set
            {
                if (isfinal) { throw new ArgumentException("The perspective is finalized. So it should not be changed."); }
                if (value == null) { throw new ArgumentNullException("value was null"); }
                if (this.XRes == 0)
                {
                    if (value.XRes > 0)
                    {
                        this.XRes = value.XRes;
                        this.YRes = value.YRes;
                        _depthMD = value;
                    }
                    else
                    {
                        throw new ArgumentException("Empty metadata object with no resolution.");
                    }
                }
                else
                {
                    if (this.XRes == value.XRes && this.YRes == value.YRes)
                    {
                        _depthMD = value;
                        this.Isfinal = true;
                    }
                    else { throw new ArgumentException("Image resolution didn't match the depth resolution."); }
                }
            }*/
        }

        private PixelColor[,] Pixels;
        public PixelColor[,] CopyPixels()
        {
            lock (this)
            {
                if (this.Pixels == null)
                {
                    BitmapSource source = this._imageBitmap;
                    if (source.Format != PixelFormats.Bgra32)
                        source = new FormatConvertedBitmap(source, PixelFormats.Bgra32, null, 0);
                    PixelColor[,] pixels = new PixelColor[source.PixelWidth, source.PixelHeight];
                    int stride = source.PixelWidth * ((source.Format.BitsPerPixel + 7) / 8);
                    GCHandle pinnedPixels = GCHandle.Alloc(pixels, GCHandleType.Pinned);
                    source.CopyPixels(
                      new Int32Rect(0, 0, source.PixelWidth, source.PixelHeight),
                      pinnedPixels.AddrOfPinnedObject(),
                      pixels.GetLength(0) * pixels.GetLength(1) * 4,
                          stride);
                    pinnedPixels.Free();
                    Pixels = pixels;
                }
                return Pixels;
            }
        }

        /// <summary>
        /// Geeft een array met rgb waarden
        /// Bestaande uit rood, groen en blue
        /// </summary>
        private ushort[] rgb = new ushort[0];
        public unsafe UInt16[] Rgb
        {

            get
            {
                // a image is usually not two pixels
                if (this.rgb.Length < 2)
                {
                    lock (this.rgb)
                    {
                        lock (this.RawImageSource)
                        {
                            this.rgb = new ushort[XRes * YRes * 3];
                            int i = 0;
                            for (uint y = 0; y < _imgMD.YRes; y++)
                            {
                                for (uint x = 0; x < _imgMD.XRes; x++)
                                {
                                    // Get a pointer to the back buffer.
                                    uint pBackBuffer = (uint)((WriteableBitmap)RawImageSource).BackBuffer;

                                    // Find the address of the pixel to draw.
                                    pBackBuffer += y * (uint)((WriteableBitmap)RawImageSource).BackBufferStride;
                                    pBackBuffer += x * 4;

                                    byte* pCol = (byte*)pBackBuffer;

                                    this.rgb[i] = pCol[2];
                                    this.rgb[i + 1] = pCol[1];
                                    this.rgb[i + 2] = pCol[0];
                                    i += 3;
                                }
                            }
                        }
                    }
                }
                    
                return rgb;
                
            }
        }

        /// <summary>
        /// Returns the image camera's bitmap source.
        /// </summary>
        public ImageSource RawImageSource
        {
            get
            {
                if (_imageBitmap == null)
                {
                    _imageBitmap = new WriteableBitmap(_imgMD.XRes, _imgMD.YRes, DPI_X, DPI_Y, PixelFormats.Rgb24, null);
                    _imageBitmap.Lock();
                    _imageBitmap.WritePixels(new Int32Rect(0, 0, _imgMD.XRes, _imgMD.YRes), _imgMD.ImageMapPtr, (int)_imgMD.DataSize, _imageBitmap.BackBufferStride);
                    _imageBitmap.Unlock();
                }
                return _imageBitmap;
            }
        }

        /// <summary>
        /// This function is depricated, but still used by the old scanner
        /// The direct access of depthMD[x] wil result in less memory usage
        /// depth[] is generated on request and cached.
        /// </summary>
        private int[] depth;
        public int[] Depth
        {
            get
            {
                if (depth == null)
                {
                    depth = new int[_depthMD.YRes * _depthMD.XRes];

                    for (int i = 0; i < _depthMD.YRes * _depthMD.XRes; i++)
                    {
                        //TODO performance opvoeren en kopieren verwijderen
                        depth[i] = _depthMD[i];
                    }
                }
                return depth;
            }
        }

        public Perspective(DateTime createTime)
        {
            this.createTime = createTime;
        }
        //private /** image*//
        //private //** depth
        /*
         * Een instantie van een perspectief bevat een moment opname van een object.
         * Concreet zijn dit de herkeningspunten, beeldmateriaal, dieptemateriaal
         * Uiteraard kunnen hier later ook spectrum data, synchrone data
         */

        public override string ToString()
        {
            return createTime.ToShortTimeString();
        }

        private unsafe void UpdateHistogram(DepthMetaData depthMD)
        {
            // Reset.
            for (int i = 0; i < Histogram.Length; ++i)
                Histogram[i] = 0;

            ushort* pDepth = (ushort*)depthMD.DepthMapPtr.ToPointer();

            int points = 0;
            for (int y = 0; y < depthMD.YRes; ++y)
            {
                for (int x = 0; x < depthMD.XRes; ++x, ++pDepth)
                {
                    ushort depthVal = *pDepth;
                    if (depthVal != 0)
                    {
                        Histogram[depthVal]++;
                        points++;
                    }
                }
            }

            for (int i = 1; i < Histogram.Length; i++)
            {
                Histogram[i] += Histogram[i - 1];
            }

            if (points > 0)
            {
                for (int i = 1; i < Histogram.Length; i++)
                {
                    Histogram[i] = (int)(256 * (1.0f - (Histogram[i] / (float)points)));
                }
            }
        }

    }
}
