echo "hello world"

# %s = string
# %d = number...
# go back to old school win command line stuff... grown too used to pscore
printf "%s\n" "hello world"
printf "%d\n" 123

# cat but feed back into stdout... <<
cat << EOF
Hello world,
This is a multiline string.
EOF

sudo apt update && sudo apt upgrade -y

# Install azure cli
curl -sL https://aka.ms/InstallAzureCLIDeb | sudo bash

# install oh-my-posh
curl -s https://ohmyposh.dev/install.sh | bash -s
oh-my-posh font install meslo
code .bashrc
eval "$(oh-my-posh --init --shell bash --config /home/dewald/.cache/oh-my-posh/themes/dewald.omp.json)"
# If oh-my-posh command not found...
export PATH=$PATH:/home/dewald/.local/bin