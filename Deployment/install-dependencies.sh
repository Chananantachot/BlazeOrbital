#!/bin/bash
set -e

# Update and install prerequisites
sudo apt-get update
sudo apt-get install -y ca-certificates curl gnupg git lsb-release

# --------------------
# Install Docker
# --------------------
sudo install -m 0755 -d /etc/apt/keyrings
curl -fsSL https://download.docker.com/linux/ubuntu/gpg | sudo gpg --dearmor -o /etc/apt/keyrings/docker.gpg
sudo chmod a+r /etc/apt/keyrings/docker.gpg

# Remove old Docker repo if present
sudo rm -f /etc/apt/sources.list.d/docker.list

CODENAME="$(. /etc/os-release && echo "$VERSION_CODENAME")"
echo "deb [arch=$(dpkg --print-architecture) signed-by=/etc/apt/keyrings/docker.gpg] https://download.docker.com/linux/ubuntu $CODENAME stable" | sudo tee /etc/apt/sources.list.d/docker.list > /dev/null

sudo apt-get update
sudo apt-get install -y docker-ce docker-ce-cli containerd.io docker-buildx-plugin docker-compose-plugin

# --------------------
# Setup blazeorbital user
# --------------------
if ! id -u blazeorbital &>/dev/null; then
  sudo useradd -m -s /bin/bash blazeorbital
fi

# Add user to docker group
sudo usermod -aG docker blazeorbital

source /home/blazeorbital/.bashrc

# --------------------
# Clone project
# --------------------
REPO_DIR="/home/blazeorbital/BlazeOrbital"
if [ ! -d "$REPO_DIR/.git" ]; then
  sudo -u blazeorbital git clone https://github.com/Chananantachot/BlazeOrbital.git "$REPO_DIR"
  cd "$REPO_DIR"
  sudo -u blazeorbital git checkout vagrant-with-Dotnet-Blazor
fi

sudo chown -R blazeorbital:blazeorbital "$REPO_DIR"

# --------------------
# Create reload script
# --------------------
sudo tee /usr/local/bin/reload-blazeorbital > /dev/null << 'EOF'
#!/bin/bash
cd /home/blazeorbital/BlazeOrbital
docker compose down
docker compose build --no-cache
docker compose up -d
EOF

sudo chmod +x /usr/local/bin/reload-blazeorbital

# Add alias for blazeorbital user
BASHRC="/home/blazeorbital/.bashrc"
ALIAS_LINE='alias reload="sudo /usr/local/bin/reload-blazeorbital"'
grep -qxF "$ALIAS_LINE" "$BASHRC" || echo "$ALIAS_LINE" >> "$BASHRC"

PROFILE="/home/blazeorbital/.profile"
SOURCE_LINE='source ~/.bashrc'
grep -qxF "$SOURCE_LINE" "$PROFILE" || echo "$SOURCE_LINE" >> "$PROFILE"
