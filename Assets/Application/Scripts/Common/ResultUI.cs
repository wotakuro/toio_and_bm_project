using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Playables;
using System.Text;
using System;


namespace BMProject
{
    public class ResultUI : MonoBehaviour
    {
        [SerializeField]
        private TextMeshPro ruleText;

        [SerializeField]
        private TextMeshPro playTimeTitleText;
        [SerializeField]
        private TextMeshPro playTimeValText;
        [SerializeField]
        private TextMeshPro scoreValText;
        [SerializeField]
        private TextMeshPro[] detailItemsText;
        [SerializeField]
        private TextMeshPro detailWholeText;

        [SerializeField]
        private GameObject btnUIRoot;

        private PlayableDirector playableDirector;



        // todo 仮
        public ResultUI SetScore(string score)
        {
            scoreValText.text = score;
            return this;
        }
        public ResultUI SetTimer(string title, string val)
        {
            this.playTimeTitleText.text = title;
            this.playTimeValText.text = val;
            return this;
        }

        public ResultUI SetRule(string rule)
        {
            this.ruleText.text = rule;
            return this;
        }

        public ResultUI SetDetail(List<string> details)
        {
            var sb = new StringBuilder(256);
            int num = detailItemsText.Length;
            for (int i = 0; i < num; ++i)
            {
                var detail = detailItemsText[i];
                detail.text = CreateLine(sb, details, i, num);
                detail.gameObject.SetActive(true);
            }
            this.detailWholeText.gameObject.SetActive(false);
            return this;
        }

        private string CreateLine(StringBuilder sb, List<string> details, int idx, int lineNum)
        {
            sb.Clear();
            for (int i = idx; i < details.Count; i += lineNum)
            {
                sb.Append(details[i]).Append("\n");
            }
            return sb.ToString();
        }

        public ResultUI SetDetail(string txt)
        {
            foreach (var detail in detailItemsText)
            {
                detail.gameObject.SetActive(false);
            }
            this.detailWholeText.gameObject.SetActive(true);
            this.detailWholeText.text = txt;
            return this;
        }

        public void StartResult()
        {
            this.gameObject.SetActive(true);
            this.playableDirector = GetComponent<PlayableDirector>();

            this.playableDirector.Evaluate();
            this.playableDirector.stopped += (d) =>
            {
                this.btnUIRoot.SetActive(true);
            };
            this.playableDirector.Play();
        }

        public void Disconnected()
        {
            this.btnUIRoot.SetActive(true);
        }
    }
}