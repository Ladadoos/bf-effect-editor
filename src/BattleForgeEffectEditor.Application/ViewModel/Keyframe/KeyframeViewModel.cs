// BattleForge Special Effect Editor
// Copyright(C) 2021 Skylords Reborn
// Project licensed under GNU General Public License v3.0. See LICENSE for more information.

using BattleForgeEffectEditor.Application.Commands;
using BattleForgeEffectEditor.Application.ViewModel.GenericControls;
using BattleForgeEffectEditor.Models;
using System;
using System.Collections.Generic;

namespace BattleForgeEffectEditor.Application.ViewModel.Keyframe
{
    public interface IKeyframeViewModel
    {
        float Index { get; set; }

        ITrackKeyFrame GetKeyFrame();
    }

    public class KeyframeViewModel<T> : ObservableObject, IKeyframeViewModel where T : ITrackKeyFrame
    {
        public float Index
        {
            get => keyFrame.Frame;
            set
            {
                keyFrame.Frame = Math.Max(value, 0);
                OnPropertyChanged();
            }
        }

        public MultiStageButtonViewModel DeleteButton { get; private set; }

        protected DynamicTrackEditorViewModel trackEditor;
        protected T keyFrame;

        public KeyframeViewModel(ITrackKeyFrame keyFrame, DynamicTrackEditorViewModel trackEditor)
        {
            this.keyFrame = (T)keyFrame;
            this.trackEditor = trackEditor;

            DeleteButton = new MultiStageButtonViewModel(new List<ButtonStage>
            {
                new ButtonStage()
                {
                    Text = "X",
                },
                new ButtonStage()
                {
                    Text = "✓",
                    FallbackTimeSeconds = 1,
                    Command = new RelayCommand((_) =>
                    {
                        trackEditor.RemoveKeyframe(this);
                    })
                },
            });
        }

        public ITrackKeyFrame GetKeyFrame() => keyFrame;
    }
}
