using GameLibrary.Dependencies.Entities;
using GameLibrary.Entities.Components;
using System;
using System.Linq;

namespace SpaceHordes.Entities.Systems
{
    public class AnimationSystem : EntityProcessingSystem
    {
        public AnimationSystem()
            : base(typeof(Animation))
        {
        }

        protected override void ProcessEntities(System.Collections.Generic.Dictionary<int, Entity> entities)
        {
            base.ProcessEntities(entities);
        }

        public override void Process(Entity e)
        {
            if (!e.HasComponent<Animation>())
                return;

            Sprite sprite = e.GetComponent<Sprite>();
            Animation anim = e.GetComponent<Animation>();

            if (anim.Type != AnimationType.None)
            {
                anim._Tick += world.Delta;

                if (anim._Tick >= anim.FrameRate)
                {
                    anim._Tick -= anim.FrameRate;
                    switch (anim.Type)
                    {
                        case AnimationType.Loop:
                            sprite.FrameIndex++; Console.WriteLine("Animation happened");
                            break;

                        case AnimationType.ReverseLoop:
                            sprite.FrameIndex--;
                            break;

                        case AnimationType.Increment:
                            sprite.FrameIndex++;
                            anim.Type = AnimationType.None;
                            break;

                        case AnimationType.Decrement:
                            sprite.FrameIndex--;
                            anim.Type = AnimationType.None;
                            break;

                        case AnimationType.Bounce:
                            sprite.FrameIndex += anim.FrameInc;
                            if (sprite.FrameIndex == sprite.Source.Count() - 1)
                                anim.FrameInc = -1;

                            if (sprite.FrameIndex == 0)
                                anim.FrameInc = 1;
                            break;

                        case AnimationType.Once:
                            if (sprite.FrameIndex < sprite.Source.Count() - 1)
                                sprite.FrameIndex++;
                            else
                                anim.Type = AnimationType.None;
                            break;
                    }
                    e.RemoveComponent<Sprite>(e.GetComponent<Sprite>());
                    e.AddComponent<Sprite>(sprite);
                }
            }
        }
    }
}