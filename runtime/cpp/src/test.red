Red [
	needs: view
]

time-series: 1
get-header: func [time-series] [[1 2 3 4]]

view [
	across int1: field data 1 int2: field data 2 return
	button "+" [res/data: form get-header time-series] return
	res: field
]