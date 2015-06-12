﻿/*
 * This is free and unencumbered software released into the public domain.
 *
 * Anyone is free to copy, modify, publish, use, compile, sell, or
 * distribute this software, either in source code form or as a compiled
 * binary, for any purpose, commercial or non-commercial, and by any
 * means.
 *
 * In jurisdictions that recognize copyright laws, the author or authors
 * of this software dedicate any and all copyright interest in the
 * software to the public domain. We make this dedication for the benefit
 * of the public at large and to the detriment of our heirs and
 * successors. We intend this dedication to be an overt act of
 * relinquishment in perpetuity of all present and future rights to this
 * software under copyright law.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
 * MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
 * IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR
 * OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
 * ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
 * OTHER DEALINGS IN THE SOFTWARE.
 *
 * For more information, please refer to <http://unlicense.org/>
 */

using System;
using Kokoro2.Math;

namespace Kokoro2.Engine.HighLevel.Voxel
{
    class QefSolver
    {
        private QefData data;
        private SMat3 ata;
        private Vector3 atb, massPoint, x;
        private bool hasSolution;

        public QefSolver()
        {
            data = new QefData();
            ata = new SMat3();
            atb = Vector3.Zero;
            massPoint = Vector3.Zero;
            x = Vector3.Zero;
            hasSolution = false;
        }

        private QefSolver(QefSolver rhs)
        { }

        public Vector3 getMassPoint()
        {
            return massPoint;
        }

        public void add(float px, float py, float pz, float nx, float ny, float nz)
        {
            hasSolution = false;

            Vector3 tmpv = new Vector3(nx, ny, nz);
            tmpv.Normalize();
            nx = tmpv.X;
            ny = tmpv.Y;
            nz = tmpv.Z;

            data.ata_00 += nx * nx;
            data.ata_01 += nx * ny;
            data.ata_02 += nx * nz;
            data.ata_11 += ny * ny;
            data.ata_12 += ny * nz;
            data.ata_22 += nz * nz;
            float dot = nx * px + ny * py + nz * pz;
            data.atb_x += dot * nx;
            data.atb_y += dot * ny;
            data.atb_z += dot * nz;
            data.btb += dot * dot;
            data.massPoint_x += px;
            data.massPoint_y += py;
            data.massPoint_z += pz;
            ++data.numPoints;
        }

        public void add(Vector3 p, Vector3 n)
        {
            add(p.X, p.Y, p.Z, n.X, n.Y, n.Z);
        }

        public void add(QefData rhs)
        {
            hasSolution = false;
            data.add(rhs);
        }

        public QefData getData()
        {
            return data;
        }

        public float getError()
        {
            if (!hasSolution)
            {
                throw new ArgumentException("Qef Solver does not have a solution!");
            }

            return getError(x);
        }

        public float getError(Vector3 pos)
        {
            if (!hasSolution)
            {
                setAta();
                setAtb();
            }

            Vector3 atax;
            MatUtils.vmul_symmetric(out atax, ata, pos);
            return Vector3.Dot(pos, atax) - 2 * Vector3.Dot(pos, atb) + data.btb;
        }

        public void reset()
        {
            hasSolution = false;
            data.clear();
        }

        public float solve(Vector3 outx, float svd_tol, int svd_sweeps, float pinv_tol)
        {
            if (data.numPoints == 0)
            {
                throw new ArgumentException("...");
            }

            massPoint.X = data.massPoint_x;
            massPoint.Y = data.massPoint_y;
            massPoint.Z = data.massPoint_z;
            massPoint *= (1.0f / data.numPoints);
            setAta();
            setAtb();
            Vector3 tmpv;
            MatUtils.vmul_symmetric(out tmpv, ata, massPoint);
            atb = atb - tmpv;
            x = Vector3.Zero;
            float result = SVD.solveSymmetric(ata, atb, x, svd_tol, svd_sweeps, pinv_tol);
            x += massPoint * 1;
            setAtb();
            outx = x;
            hasSolution = true;
            return result;
        }

        private void setAta()
        {
            ata.setSymmetric(data.ata_00, data.ata_01, data.ata_02, data.ata_11, data.ata_12, data.ata_22);
        }

        private void setAtb()
        {
            atb.X = data.atb_x;
            atb.Y = data.atb_y;
            atb.Z = data.atb_z;
        }



    }
}