# Intro

The optimal solution can be constructed by optimal solutions by sub problems.
Lets use divide and conquer with dynamic programming technique.

# Describe what it does / Idea

Given cards (where ... is any number of cards) and it's the ronald to play.
9 3 ... 4
Ronald will pick the largest card which is 9.
Then either 3 or 4 can be picked, but whichever the optimal solution is one of them plus the solution to the problem without the 2 taken cards.

This means the solution can be constructed by a chain of n/2 optimal solutions of sub problems.


Short and sweet

# Solution

Given n as total amount of cards.
Given C as a list of n integers
Define M as memory of solutions to sub problems.


Define h as head index pointing to the first card.
Define t as tail index pointing to the last card.

x x 1 2 3 4 5 x x
    h       t


Define S(h,t)
S(h, t) = 
    M[h,t]                                          if M[h,t] exists
    C_h                                             if h = t
    Max(C_h, C_t)                                   if |h-t| = 1
    Max(C_(t-1) + S(h, t-2), C_h + S(h+1,t-1))      if C_h < C_t
    Max(C_t + S(h+1,t-1), C_(h+1) + S(h+2,t))       if C_h > C_t
    Max(
      C_(t-1) + S(h, t-2), 
      C_h + S(h+1,t-1),
      C_t + S(h+1,t-1), 
      C_(h+1) + S(h+2,t)
    )                                               otherwise

All optimal solutions calculated by a succesfull evaluation of S(h,t) will be saved to memory.
M[h,t] = S(h,t) 

Then solve
for n > 1 Max(C_0 + S(1,n),C_n + S(0,n-1))
otherwise C_0

# Correctness

# Time complexity

Analyse by theory.
Analyse by practise.

# Space complexity

one integer given as n
n integers given as C
n*n integers saved in M.
asymptotical: O(n^2)