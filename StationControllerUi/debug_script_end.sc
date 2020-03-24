
label SendData cmnd,data
	send name=debug_connection $cmnd:$data
	return

label debug_connection
	let command = get_tmp_var(ARG0)
	print "DEBUG command: ":$command
	if($command eq "label")
		let parameter = get_tmp_var(ARG1)
		goto $parameter
	end_if