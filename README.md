# Pointillism_image_generator

First things first, the main source for this project was paper by Hiroki Tokura, Yuki Kuroda, Yasuaki Ito, and Koji Nakano from Hiroshima University. The paper can be found here: https://www.google.com/url?sa=t&rct=j&q=&esrc=s&source=web&cd=&ved=2ahUKEwijn8-3i_j5AhVO6qQKHQPzDxsQFnoECAkQAQ&url=https%3A%2F%2Fwww.cs.hiroshima-u.ac.jp%2Fcs%2F_media%2Fcandar17_pointilism.pdf&usg=AOvVaw2gcvkMONkN1JjwVYe-icVg

 (They propose a far more magnificent generator than is this one.)

Pointillism is a technique of painting, where small dots of color are put on a canvas. These dots are distinct and altogether create the pointillistic image. This technique was developed in 1886 by Georges Seurat and Paul Signac.

This program aims to generate a digital version of pointillistic image from an input image. Different patterns could be used, such as dots, ellipses, or squares. This program uses square patterns, which can be rotated about centre for 30 or 60 degrees. The size of pattern is selected by the user, the smallest pattern’s width consists of 7 pixels, the largest of 13. 

The result depends on the size and the number of patterns used. Because of the algorithm, details in the input image, which are smaller than a single pattern, cannot be reproduced well. Consequently, the output image looks abstract and blurred.

# Algorithm and data structures

To explain the algorithm, error and improvement must be introduced first. Error of output image is computed as a difference between every pixel in the input image and the matching pixel in the output image, for each RGB channel separately. Mathematically, error of images $A$ and $B$ of size $N$x$N$ is $$Error(A,B) =  \sum_{1 \leq i, j \leq N} \left( |e^R_{(i,j)}| + |e^G_{(i,j)}| + |e^B_{(i,j)}| \right)$$, where $e_{(i,j)} = A_{i,j} - B_{i,j}$.

Improvement is a difference between the output picture before and after a pattern is pasted. So, it defines which pattern to paste. For images $A$ and $B$, improvement after pattern $p$ was placed in image $B$ on index $i,j$ is: $$Improvement(A, B, p, i, j) = Error(A, B) − Error(A, B′) $$, where $B′$ is an image where a square pattern $p$ is pasted at $(i, j)$ to $B$.

The algorithm is very briefly following. First, for each index of image*, best pattern is found. By the best pattern for index, we mean a pattern with the smallest error, which is affected by the combination of color and rotation. Then, a pattern with the greatest improvement of all “best patterns for index” is placed in the image. All patterns located close enough to this newly placed pattern need to be updated. 

After no improvement is possible, that is when the best of the best patterns has no improvement, algorithm ends. 

For these purposes I’m using max heap data structure to quickly find a pattern with the greatest improvement. Because the patterns in the heap need to be updated, to do it in a reasonable time, I also use a dictionary. The key is an index of a pattern, and the value is a position of the pattern in the heap. 

*in this implementation, for only every 16th index best pattern is found – it decreases precision but also memory requirements

# Warning
Program is very slow. Experimental time needed to paste 1000 number of patterns (7 pixels in size) is around 50 seconds. High resolution images are not recommended as an input picture, the details still will not be preserved.

Larger images means longer intitalization. Bigger size of patterns causes longer image generation.
# GUI
$Load$ an image and choose a size of pattern. Decide, whether you want to save the process of image generation and press $Start$ button, first 500 patterns will be pasted. You can $Add$ more patterns by clicking a button. If you have checked $Save$ $progress$, use track bar to view it. Any currently displayed image you can $Save$.
![image](https://user-images.githubusercontent.com/108612296/188353463-a396dbb4-8a7b-48f3-84a9-f5e374ddb221.png)

# Result
![image](https://user-images.githubusercontent.com/108612296/188346127-f31b935a-2269-4758-8476-cf292c58c249.png)

Input image: Lena. Size of pattern: 11. Number of patterns: 11500.
