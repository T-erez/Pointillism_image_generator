# Pointillism
First things first, the main source for this project is paper by Hiroki Tokura, Yuki Kuroda, Yasuaki Ito, and Koji Nakano from Hiroshima University. The paper can be found here: https://www.google.com/url?sa=t&rct=j&q=&esrc=s&source=web&cd=&ved=2ahUKEwijn8-3i_j5AhVO6qQKHQPzDxsQFnoECAkQAQ&url=https%3A%2F%2Fwww.cs.hiroshima-u.ac.jp%2Fcs%2F_media%2Fcandar17_pointilism.pdf&usg=AOvVaw2gcvkMONkN1JjwVYe-icVg

Pointillism is a technique of painting, where small dots of color are put on canvas. These dots are distinct and altogether create the pointillistic image. This technique was developed in 1886 by Georges Seurat and Paul Signac.

![Paul_Signac_Femmes_au_puits_1892détailcouleur](https://github.com/T-erez/Pointillism_image_generator/assets/108612296/125eea41-9ec6-40d1-b303-2a67723607c7)

Femmes au puits, by Paul Signac, 1892. Shows a detail of an image. (https://commons.wikimedia.org/w/index.php?curid=5950432)

# Pointillism image generator

This program aims to generate a digital version of pointillistic image from any input image. Different patterns could be used, such as dots, ellipses, or squares. This program uses square patterns, which can be rotated about centre for 30 or 60 degrees. The size of the patterns is chosen by the user, supported sizes range from 7 to 23 pixels. 

The appearance of the resulting image depends on:
- The size of patterns. Because of the algorithm, details in the input image, which are smaller than a single pattern, cannot be reproduced well. Therefore, the larger the patterns, the more abstract and blurred the image.
- The number of patterns. Generator adds patterns only if it improves the generated image (see below what exactly the improvement value means). The more patterns, the more precise the generated image.
- The background color. Generator first adds patterns that contrast more with the background color. The background color is usually visible in the generated image.

# Snapshots of pasting patterns
![progress](https://github.com/T-erez/Pointillism_image_generator/assets/108612296/6d383608-0597-4e65-bad4-ebd7610a4dcd)

# Serial algorithm

To explain the algorithm, error and improvement must be introduced first. Error of the generated image is computed as a difference between every pixel in the input image and the matching pixel in the generated image, for each RGB channel separately. Mathematically, error of images $A$ and $B$ of size $N$ x $N$ is $$Error(A,B) =  \sum_{1 \leq i, j \leq N} \left( |e^R_{(i,j)}| + |e^G_{(i,j)}| + |e^B_{(i,j)}| \right)$$, where $e_{(i,j)} = A_{i,j} - B_{i,j}$.

To compare error of patterns in the same location, it is enough to compute error of affected region (also referred to as window) where fits pattern with any rotation.

Improvement is a difference between the error of generated image before and after a pattern is added. As we want to minimize the error, patterns with larger improvement are added first. For images $A$ and $B$, improvement after pattern $p$ was added to image $B$ on index $(i,j)$ is: $$Improvement(A, B, p, i, j) = Error(A, B) − Error(A, B′) $$, where $B′$ is image $B$ with a square pattern $p$ added at $(i, j)$.

The serial algorithm is very briefly following. First, for each index of image*, the best pattern is found. By the best pattern on an index, we mean a pattern centred on that index and with such a combination of color and rotation that it improves the generated image the most. Then, the best pattern of all “best patterns on an index” is added to the image. All patterns located close enough to this newly added pattern need to be updated.

Algorithm ends after no improvement is possible, that is when the best of best patterns has non positive improvement value.

*In this implementation, the best pattern is only found for every 4th index - it reduces memory requirements and improves performance while maintaining accuracy.

# Parallel algorithm
In the above serial algorithm, patterns are pasted one by one. For adding multiple patterns at once, there is a non-deterministic parallel algorithm. To add multiple patterns, the input
image $A$ of size $N$ x $N$ is split into subimages of size $h$ x $h$. The subimages are divided into four groups such that:
- Group 1 are subimages on even columns and even rows
- Group 2 are subimages on odd columns and even rows
- Group 3 are subimages on even columns and odd rows
- Group 4 are subimages on odd columns and odd rows

For parallel execution, the subimage has to be equal to or greater than the affected region.

The disadvantage of this approach is that a generated image after approximately 40000 pasted squares may look like this:
![without_improvementLevel](https://github.com/T-erez/Pointillism_image_generator/assets/108612296/5227f915-66f4-48bd-9020-0e1c67deed19)

The borders of subimages are visible and the image looks unevenly generated. This does not matter if the goal is to cover background as much as possible. 
But sometimes it looks nice when there is only an outline of the main object and a lot of background is visible.
To eliminate this problem and to make the parallel algorithm look more similar to the serial one, the lowest allowed improvement is defined. Only patterns with greater improvement value than the current lowest allowed improvement can be added. 
This causes that patterns added at the same time have a similar color or contrast similarly with the background.

The generated image (with about 40000 pasted squares) after introducing lowest allowed improvement: 
![with_improvementLevel](https://github.com/T-erez/Pointillism_image_generator/assets/108612296/056b9272-1146-4614-aedd-02973e056b9e)


# Data structures
A pattern with the greatest improvement has to be found quickly. Therefore, as a data structure for patterns, max heap is used. 
Since patterns in the heap often need to be updated, a dictionary is used to find a pattern in the heap faster. The key is the centre of the pattern, and the value is its position in the heap. 

# Decomposition

The most important is ``PointillismImageGenerator``, an abstract class that generates a pointillistic image. The only public methods are ``AddPatterns()`` and ``Dispose()``. ``PointillismImageGeneratorParallel``  and ``PointillismImageGeneratorSerial``are derived class that implement the parallel (serial) pasting algorithm.

``Subimages`` is a class used by parallel generator, that splits an image into subimages and divides them into groups.

Structure ``PatternWithImprovement`` represents a pattern, its error and improvement value. Structure implements IComparable interface. For every two instances, the one with greater improvement value precedes the other.

``SquarePattern`` is a structure for a square pattern with its color, angle and location in the generated image. 


# GUI
In the setup, $Load$ an image, select a size of patterns and set background color. Then press $Start$ button to initialize the generator and to add first 100 patterns. You can $Add$ more patterns by clicking a button. Adding patterns can be cancelled by $Cancel$ button. If you have checked $Save$ $progress$, 5 images during generation will be saved. Use track bar to view generated images. Any currently displayed image you can $Save$.

To try different pattern size or background color, simply change them in setup and click $Restart$. 
![new_design_III](https://github.com/T-erez/Pointillism_image_generator/assets/108612296/a32eac94-1bbb-4539-bae7-cf1e82d6b8e2)


# Speed stats
Experimental speed are:

* 30000 patterns, 7 pixels in size. Parallel: 0:21, serial: 0:48.
* 30000 patterns, 9 pixels in size. Parallel: 0:47, serial: 2:02.
* 30000 patterns, 11 pixels in size. Parallel: 1:59, serial: 4:55.
* 10000 patterns, 23 pixels in size. Parallel: 8:06. 

The larger the images, the longer the initialization. The larger the patterns, the longer the image generation.

# Results

All results are generated by ``PointillismImageGeneratorParallel``.

- Input image: Golden Tears by Gustav Klimt (570x761). (https://mucha-alphonse.eu/en/long/353-golden-tears.html)

Size of patterns: 11. Number of patterns: 10118. Background color: yellow.
![10118](https://github.com/T-erez/Pointillism_image_generator/assets/108612296/2681b564-85c2-48c7-8604-d1413aa778ba)

Size of patterns: 11. Number of patterns: 5712. Background color: rgb(250, 231, 189).
![5712](https://github.com/T-erez/Pointillism_image_generator/assets/108612296/b97b233a-c4b2-4b35-8c4c-76c9a836e0ed)


- Input image: Autumn park (2560x1600) (https://www.10wallpaper.com/view/Autumn_park_foliage_benches_trees-2016_Scenery_HD_Wallpaper.html)

Size of patterns: 11. Number of patterns: 170156. Background color: rgb(250, 231, 189).
![170156](https://github.com/T-erez/Pointillism_image_generator/assets/108612296/87c19a0d-d9b8-47d1-a2e1-60535b5e077c)


-Input image: Two goldfish (612x553). (https://www.freepik.com/premium-photo/two-goldfish-fish-bowl-isolated_8784444.htm) 

Size of patterns: 7. Number of patterns: 23820. Background color: white.
![23820](https://github.com/T-erez/Pointillism_image_generator/assets/108612296/a361c0c9-9134-460f-82ea-7b39f2fb66fc)


- Input image: a friend's cat (1448x1440).

Size of patterns: 7. Number of patterns: 45162. Background color: rgb(34, 143, 29) or very similar.
![45162](https://github.com/T-erez/Pointillism_image_generator/assets/108612296/671b55ea-374c-43cd-bf8a-f31e16800018)

Size of patterns: 7. Number of patterns: 145302. Background color: rgb(34, 143, 29) or very similar.
![145302](https://github.com/T-erez/Pointillism_image_generator/assets/108612296/aacb4c5f-1ff0-474e-a49b-286e57c69f81)


