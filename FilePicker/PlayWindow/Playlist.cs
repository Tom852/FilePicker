﻿using FilePicker.Scanner;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilePicker.PlayWindow
{
    public class Playlist : INotifyPropertyChanged
    {
        private const int PlayListSize = 50;
        private FileChooser fileChooser;
        private LinkedList<FileRepresentation> data = new LinkedList<FileRepresentation>();
        private LinkedListNode<FileRepresentation> currentNode;

        public Playlist(FileChooser chooser)
        {
            this.fileChooser = chooser;
            PopulatePlaylist();
        }


        public FileRepresentation PrevPrev => currentNode.Previous?.Previous?.Value;
        public FileRepresentation Prev => currentNode.Previous?.Value;
        public FileRepresentation Current => currentNode.Value;
        public FileRepresentation Next => currentNode.Next?.Value;
        public FileRepresentation NextNext => currentNode.Next?.Next?.Value;


        public event PropertyChangedEventHandler PropertyChanged;

        public void RotateForward()
        {

            currentNode = currentNode.Next;
            PopulatePlaylist();
            NotifyAll();
        }

        public void RotateBackward()
        {
            if (currentNode.Previous != null)
            {
                currentNode = currentNode.Previous;
            }
            NotifyAll();
        }

        public async Task<LinkedList<FileRepresentation>> Add50FilesFromCurrentAndReturnWithoutRotating()
        {
            return await Task.Run(() =>
            {

                LinkedList<FileRepresentation> result = new LinkedList<FileRepresentation>();
                var n = this.currentNode;
                result.AddLast(n.Value);

                for (int i = 0; i < PlayListSize; i++)
                {
                    if (n.Next is null)
                    {
                        n = this.data.AddLast(this.fileChooser.ChooseFile());
                    }
                    else
                    {
                        n = n.Next;
                    }
                    result.AddLast(n.Value);
                }
                return result;
            });
        }

        private void PopulatePlaylist()
        {
            if (currentNode == null)
            {
                currentNode = this.data.AddFirst(this.fileChooser.ChooseFile());
            }

            while (currentNode.Next?.Next == null)
            {
                this.data.AddLast(this.fileChooser.ChooseFile());
            }
            NotifyAll();
        }

        private void NotifyAll()
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PrevPrev)));
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Prev)));
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Current)));
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Next)));
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NextNext)));
        }
    }
}
