-- Get total documents
SELECT COUNT(1) FROM c


-- Get current location from the location container
SELECT TOP 1 * FROM c
WHERE c.flightNumber = '2021'
ORDER BY c._ts DESC


-- Get delivery board
SELECT * FROM c 
WHERE c.type = 'location' 
AND c.arrivalCity IN('YYT','YQY','YYG','YHZ','YFC','YFB','YQB','YUL','YOW','YYZ','YAM','YQT','YWG','YQR','YXE','YZF','YEG','YCG','YVR','YYJ','YXS','YXY','ANC')
