using System;
using System.Numerics;

namespace SPHLionLIB
{
    /// <summary>
    /// Частица
    /// </summary>
    public class Particle_Solid : Particle
    
    {
        public new float Density
        {
            get { return density; }
            set
            {
                density = value;
             }
        }

        public float Pmax;


        /// <summary>
        /// Конструктор для массивов
        /// </summary>
        public Particle_Solid()
        {
        }
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="Position">Позиция</param>
        /// <param name="Velocity">Скорость</param>
        public Particle_Solid(Vector2 Position, Vector2 Velocity, float Dns, float P)
        {
            this.Velocity = Velocity;
            this.Position = Position;
            PositionOld = Position;
            Density = Dns;
            Pressure = P;
        }
 
    }
}
