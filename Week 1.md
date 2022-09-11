# Week 1

## 1.1
Level 0: C*sqrt(n)
Level 1: C*sqrt(n/4) + C*sqrt(n/4) = 2(C*sqrt(n/4)) = C * sqrt(n)
Level 2: 4*(C*sqrt((n/4)/4)) 
Level 3: 8*(C*sqrt(((n/4)/4)/4)) = C * sqrt(n)

Thus bounded by O(n log n)

## 1.2
Level 0: cn
Level 1: 2(c*(n/4)) = cn/2
Level 2: 4(c*((n/4)/4)) = cn/4
Level 3: 8(c*(((n/4)/4)/4)) = cn/8



## 1.3
Level 0: cn^2
Level 1: 2(c*(n/4)^2) = cn^2/8
Level 2: 4(c*((n/4)/4)^2) = cn^2/64
Level 3: 8(c*(((n/4)/4)/4)^2) = cn^2 / 512

## 1.4
Level 0: cn
Level 1: c(n*3/4) = c*n*3/4
level 2: c(n*3/4*3/4) = c*n*9/16


## 1.5


## 3.1
200 both from day 1. 
200 buying for 300 day 4 and selling for 500 day 5

## 3.2





## 3.3
Split all the way to 2 entries -> run the algorithm with the first day. Either get a high profit and keep index one forever,
or get 0 and 