# Pointillism_image_generator

First things first, the main source for this project was paper by Hiroki Tokura, Yuki Kuroda, Yasuaki Ito, and Koji Nakano from Hiroshima University. The paper can be found here: https://www.google.com/url?sa=t&rct=j&q=&esrc=s&source=web&cd=&ved=2ahUKEwijn8-3i_j5AhVO6qQKHQPzDxsQFnoECAkQAQ&url=https%3A%2F%2Fwww.cs.hiroshima-u.ac.jp%2Fcs%2F_media%2Fcandar17_pointilism.pdf&usg=AOvVaw2gcvkMONkN1JjwVYe-icVg

 (They propose a far more magnificent generator than is this one.)

Pointillism is a technique of painting, where small dots of color are put on canvas. These dots are distinct and altogether create the pointillistic image. This technique was developed in 1886 by Georges Seurat and Paul Signac.

This program aims to generate a digital version of pointillistic image from an input image. Different patterns could be used, such as dots, ellipses, or squares. This program uses square patterns, which can be rotated about centre for 30 or 60 degrees. The size of pattern is selected by the user, the smallest pattern’s width consists of 7 pixels, the largest of 13. 

The result depends on the size and the number of patterns used. Because of the algorithm, details in the input image, which are smaller than a single pattern, cannot be reproduced well. Consequently, the output image looks abstract and blurred.

# Algorithm and data structures

To explain the algorithm, error and improvement must be introduced first. Error of output image is computed as a difference between every pixel in the input image and the matching pixel in the output image, for each RGB channel separately. Mathematically, error of images $A$ and $B$ of size $N$x$N$ is $$Error(A,B) =  \sum_{1 \leq i, j \leq N} \left( |e^R_{(i,j)}| + |e^G_{(i,j)}| + |e^B_{(i,j)}| \right)$$, where $e_{(i,j)} = A_{i,j} - B_{i,j}$.

Improvement is a difference between the error of output picture before and after a pattern is pasted. So, it defines which pattern to paste. For images $A$ and $B$, improvement after pattern $p$ was placed in image $B$ on index $i,j$ is: $$Improvement(A, B, p, i, j) = Error(A, B) − Error(A, B′) $$, where $B′$ is an image where a square pattern $p$ is pasted at $(i, j)$ to $B$.

The algorithm is very briefly following. First, for each index of image*, best pattern is found. By the best pattern for index, we mean a pattern with the smallest error, which is affected by the combination of color and rotation. Then, a pattern with the greatest improvement of all “best patterns for index” is placed in the image. All patterns located close enough to this newly placed pattern need to be updated. 

After no improvement is possible, that is when the best of the best patterns has no improvement, algorithm ends. 

For these purposes I’m using max heap data structure to quickly find a pattern with the greatest improvement. Because the patterns in the heap need to be updated, to do it in a reasonable time, I also use a dictionary. The key is an index of a pattern, and the value is a position of the pattern in the heap. 

*in this implementation, for only every 16th index best pattern is found – it decreases precision but also memory requirements

# Decomposition

Class ``Pattern`` represents a pattern to be added to the generated image. Patern uniqueness is determined by the characteristics of color, angle and location on the generated image. 

Class ``Node`` holds pattern with its error, improvement value and background contribution. Background contribution is a number of background pixels covered by pattern. The other parts of program does not take advantage of background contribution variable, but it could be used for covering background pixels first, so that background does not peak out from the image. Class supports comparing Nodes with each other. Node with larger background contribution is always larger. When the background contribution is equal, Node with larger improvement value is larger.

For storing Nodes, there is a class ``SmartHeap``. It consists of max heap with Nodes and dictionary to find Nodes in heap quickly. Methods included are: ``Add()`` to add new element to heap, ``GetMax()`` to get the largest Node, ``Change()`` to change a Node in heap.

And the most important class is ``Pointillism`` which generates a pointillistic image. The initialization is done by ``Initialize()`` method. ``GeneratePointillismImage()`` adds one pattern to the generated image. To get the generated image use ``GetOutputImage()``.


# GUI
$Load$ an image and choose a size of pattern. Decide, whether you want to save the process of image generation and press $Start$ button, first 500 patterns will be pasted. You can $Add$ more patterns by clicking a button. If you have checked $Save$ $progress$, use track bar to view it. Any currently displayed image you can $Save$.
![image](https://user-images.githubusercontent.com/108612296/188353463-a396dbb4-8a7b-48f3-84a9-f5e374ddb221.png)

# Warning
Program is very slow. Experimental speed of pasting patterns is:

* for a pattern 7 pixels in size: 400+ patterns per minute
* for a pattern 11 pixels in size: around 150 patterns per minute
* for a pattern 13 pixels in size: around 45 patterns per minute. 

Larger images means longer intitalization. Bigger size of patterns causes longer image generation.

# Results
![image](https://user-images.githubusercontent.com/108612296/188346127-f31b935a-2269-4758-8476-cf292c58c249.png)

Input image: Lena (512x512). Size of pattern: 11. Number of patterns: 11500.

![image](https://user-images.githubusercontent.com/108612296/188744479-daac6ffe-1fe0-4581-9eb0-6f2d656071d9.png)

Input image: Flower Paradise by Caroline Ashwood (1600x1330). Size of pattern: 7. Number of patterns: 36500.

![image](https://user-images.githubusercontent.com/108612296/188745959-a0bec154-bccf-41e4-84d6-2a6b936f2fb2.png)

Input image: A friend's cat (1448x1440). Size of pattern: 7. Number of patterns: 67000.
