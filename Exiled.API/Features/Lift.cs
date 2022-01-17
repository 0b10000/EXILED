// -----------------------------------------------------------------------
// <copyright file="Lift.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Exiled.API.Features
{
    using System.Collections.Generic;
    using System.Linq;

    using Exiled.API.Enums;
    using Exiled.API.Structs;

    using UnityEngine;

    using BaseLift = global::Lift;

    /// <summary>
    /// The in-game lift.
    /// </summary>
    public class Lift
    {
        private readonly List<Elevator> elevators = new List<Elevator>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Lift"/> class.
        /// </summary>
        /// <param name="baseLift">The <see cref="BaseLift"/> to wrap.</param>
        internal Lift(BaseLift baseLift)
        {
            Base = baseLift;

            foreach (BaseLift.Elevator elevator in baseLift.elevators)
                elevators.Add(new Elevator(elevator));
        }

        /// <summary>
        /// Gets the base <see cref="BaseLift"/>.
        /// </summary>
        public BaseLift Base { get; }

        /// <summary>
        /// Gets the lift's name.
        /// </summary>
        public string Name => Base.elevatorName;

        /// <summary>
        /// Gets the lift's <see cref="UnityEngine.GameObject"/>.
        /// </summary>
        public GameObject GameObject => Base.gameObject;

        /// <summary>
        /// Gets the lift's position.
        /// </summary>
        public Vector3 Position => Base.transform.position;

        /// <summary>
        /// Gets the lift's rotation.
        /// </summary>
        public Quaternion Rotation => Base.transform.rotation;

        /// <summary>
        /// Gets or sets the lift's <see cref="BaseLift.Status"/>.
        /// </summary>
        public BaseLift.Status Status
        {
            get => (BaseLift.Status)Base.NetworkstatusID;
            set => Base.SetStatus((byte)value);
        }

        /// <summary>
        /// Gets the lift's <see cref="ElevatorType"/>.
        /// </summary>
        public ElevatorType Type
        {
            get
            {
                switch (Name)
                {
                    case "SCP-049":
                        return ElevatorType.Scp049;
                    case "GateA":
                        return ElevatorType.GateA;
                    case "GateB":
                        return ElevatorType.GateB;
                    case "ElA":
                    case "ElA2":
                        return ElevatorType.LczA;
                    case "ElB":
                    case "ElB2":
                        return ElevatorType.LczB;
                    case "":
                        return ElevatorType.Nuke;

                    default:
                        return ElevatorType.Unknown;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the lift is operative.
        /// </summary>
        public bool IsOperative => Base.operative;

        /// <summary>
        /// Gets or sets a value indicating whether the lift is locked.
        /// </summary>
        public bool IsLocked
        {
            get => Base.Network_locked;
            set => Base.Network_locked = value;
        }

        /// <summary>
        /// Gets a value indicating whether the lift is lockable.
        /// </summary>
        public bool IsLockable => Base.lockable;

        /// <summary>
        /// Gets or sets the lift's moving speed.
        /// </summary>
        public float MovingSpeed
        {
            get => Base.movingSpeed;
            set => Base.movingSpeed = value;
        }

        /// <summary>
        /// Gets or sets the maximum distance from which an object should be considered inside the lift's range.
        /// </summary>
        public float MaxDistance
        {
            get => Base.maxDistance;
            set => Base.maxDistance = value;
        }

        /// <summary>
        /// Gets a <see cref="IEnumerable{T}"/> of <see cref="Vector3"/> which contains all the lift's cached items' position.
        /// </summary>
        public IEnumerable<Vector3> CachedItemPositions => Base._cachedItemTransforms.Select(item => item.position);

        /// <summary>
        /// Gets a <see cref="IEnumerable{T}"/> of <see cref="Vector3"/> which contains all the lift's cached items' rotation.
        /// </summary>
        public IEnumerable<Quaternion> CachedItemRotations => Base._cachedItemTransforms.Select(item => item.rotation);

        /// <summary>
        /// Gets a <see cref="IEnumerable{T}"/> of <see cref="Vector3"/> which contains all the lift's cached items' <see cref="Transform"/>.
        /// </summary>
        public IEnumerable<Transform> CachedItemTransformss => Base._cachedItemTransforms;

        /// <summary>
        /// Gets a <see cref="IEnumerable{T}"/> of <see cref="Elevator"/> which contains all the lift's elevators.
        /// </summary>
        public IEnumerable<Elevator> Elevators => elevators;

        /// <summary>
        /// Gets a <see cref="IEnumerable{T}"/> of <see cref="Lift"/> which contains all the <see cref="Lift"/> instances from the specified <see cref="BaseLift.Status"/>.
        /// </summary>
        /// <param name="status">The specified <see cref="BaseLift.Status"/>.</param>
        /// <returns>A <see cref="Lift"/> or <see langword="null"/> if not found.</returns>
        public static IEnumerable<Lift> Get(BaseLift.Status status) => Map.Lifts.Where(lift => lift.Status == status);

        /// <summary>
        /// Gets the <see cref="Lift"/> belonging to the <see cref="BaseLift"/>, if any.
        /// </summary>
        /// <param name="baseLift">The <see cref="BaseLift"/> instance.</param>
        /// <returns>A <see cref="Lift"/> or <see langword="null"/> if not found.</returns>
        public static Lift Get(BaseLift baseLift) => Map.Lifts.FirstOrDefault(lift => lift.Base == baseLift);

        /// <summary>
        /// Gets the <see cref="Lift"/> corresponding to the specified <see cref="ElevatorType"/>, if any.
        /// </summary>
        /// <param name="type">The <see cref="ElevatorType"/>.</param>
        /// <returns>A <see cref="Lift"/> or <see langword="null"/> if not found.</returns>
        public static Lift Get(ElevatorType type) => Map.Lifts.FirstOrDefault(lift => lift.Type == type);

        /// <summary>
        /// Gets the <see cref="Lift"/> corresponding to the specified name, if any.
        /// </summary>
        /// <param name="name">The lift's name.</param>
        /// <returns>A <see cref="Lift"/> or <see langword="null"/> if not found.</returns>
        public static Lift Get(string name) => Map.Lifts.FirstOrDefault(lift => lift.Name == name);

        /// <summary>
        /// Gets the <see cref="Lift"/> belonging to the <see cref="UnityEngine.GameObject"/>, if any.
        /// </summary>
        /// <param name="gameObject">The <see cref="UnityEngine.GameObject"/>.</param>
        /// <returns>A <see cref="Lift"/> or <see langword="null"/> if not found.</returns>
        public static Lift Get(GameObject gameObject) => Map.Lifts.FirstOrDefault(lift => lift.GameObject == gameObject);

        /// <summary>
        /// Tries to melt a <see cref="Player"/>.
        /// </summary>
        /// <param name="player">The <see cref="Player"/> to melt.</param>
        /// <returns><see langword="true"/> if the player was melted successfully; otherwise, <see langword="false"/>.</returns>
        public static bool TryMeltPlayer(Player player)
        {
            if (player.Position.y >= 200 || player.Position.y <= -200)
                return false;

            player.EnableEffect(EffectType.Decontaminating);

            return true;
        }

        /// <summary>
        /// Plays the lift's music.
        /// </summary>
        public void PlayMusic() => Base.RpcPlayMusic();

        /// <summary>
        /// Tries to start the lift.
        /// </summary>
        /// <returns><see langword="true"/> if the lift was started successfully; otherwise, <see langword="false"/>.</returns>
        public bool TryStart() => Base.UseLift();
    }
}