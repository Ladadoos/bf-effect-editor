// BattleForge Special Effect Editor
// Copyright(C) 2021 Skylords Reborn
// Project licensed under GNU General Public License v3.0. See LICENSE for more information.

using BattleForgeEffectEditor.Application.Commands;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Threading;

namespace BattleForgeEffectEditor.Application.ViewModel.GenericControls
{
    public struct ButtonStage
    {
        public string Text { get; set; }
        public ICommand Command { get; set; }
        public double FallbackTimeSeconds { get; set; }
    }

    public class MultiStageButtonViewModel : ObservableObject
    {
        public string Text => stages[stageNumber].Text;

        public ICommand Command => new RelayCommand((_) => OnClick());

        private DispatcherTimer timer = new DispatcherTimer();
        private List<ButtonStage> stages;
        private int stageNumber = 0;

        public MultiStageButtonViewModel(List<ButtonStage> stages)
        {
            if (stages.Count == 0)
                throw new Exception("Multi stage button must contain atleast one stage.");

            this.stages = stages;
        }

        private void OnClick()
        {
            stages[stageNumber].Command?.Execute(null);
            GoToNextStage();
            UpdateFallbackTimer();
        }

        private void GoToNextStage()
        {
            stageNumber++;
            if (stageNumber >= stages.Count)
                stageNumber = 0;
            RaisePropertyChanged(() => Text);
        }

        private void GoToPreviousStage()
        {
            stageNumber = Math.Max(stageNumber - 1, 0);
            RaisePropertyChanged(() => Text);
        }

        private void UpdateFallbackTimer()
        {
            ButtonStage stage = stages[stageNumber];
            if (stage.FallbackTimeSeconds == 0)
            {
                timer.Stop();
            } else
            {
                timer.Interval = TimeSpan.FromSeconds(stage.FallbackTimeSeconds);
                timer.Tick += (sender, args) =>
                {
                    GoToPreviousStage();
                };
                timer.Start();
            }
        }
    }
}
